namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Castle.Core.Internal;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new();
            string rootName = "Creators";
            CreatorImportDto[] creatorsDtos = xmlHelper.Decerialize<CreatorImportDto[]>(xmlString, rootName);
            ICollection<Creator> creators = new HashSet<Creator>();

            foreach (var dto in creatorsDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                ICollection<Boardgame> boargames = new HashSet<Boardgame>();
                foreach (var bg in dto.Boardgames)
                {
                    if (!IsValid(bg))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var boardGame = new Boardgame()
                    {
                        Name = bg.Name,
                        Rating = bg.Rating,
                        YearPublished = bg.YearPublished,
                        CategoryType = (CategoryType)bg.CategoryType,
                        Mechanics = bg.Mechanics
                    };
                    boargames.Add(boardGame);
                }
                var creator = new Creator()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Boardgames = boargames
                };
                creators.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator,
                    creator.FirstName,
                    creator.LastName,
                    creator.Boardgames.Count()));
            }
            context.Creators.AddRange(creators);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new();
            SellerImportDto[] sellerImportDtos = JsonConvert.DeserializeObject<SellerImportDto[]>(jsonString);

            ICollection<Seller> sellers = new HashSet<Seller>();

            var allBoardgameIds = context.Boardgames
             .AsNoTracking()
             .Select(e => new
             {
                 e.Id
             })
             .ToArray();

            foreach (var dto in sellerImportDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Seller seller = new()
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    Country = dto.Country,
                    Website = dto.Website,
                    BoardgamesSellers = new HashSet<BoardgameSeller>()
                };
                foreach (var id in dto.Boardgames)
                {
                    if (!allBoardgameIds.Any(x=>x.Id==id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    BoardgameSeller boardgameSeller = new()
                    {
                        BoardgameId = id
                    };
                    seller.BoardgamesSellers.Add(boardgameSeller);
                }
                sellers.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count()));
            }
            context.Sellers.AddRange(sellers);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
