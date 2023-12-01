namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utilities;
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creators = context.Creators
                .ToArray()
                .Where(x => x.Boardgames.Any())
                .Select(e => new
                {
                    Name = string.Join(' ', e.FirstName, e.LastName),
                    BoardgamesCount = e.Boardgames.Count(),
                    Boardgames = e.Boardgames
                    .Select(b => new
                    {
                        BoardgameName = b.Name,
                        BoardgameYearPublished = b.YearPublished
                    })
                    .OrderBy(n => n.BoardgameName)
                    .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(n => n.Name)
                .ToArray();

            HashSet<CreatorExportDto> creatorExportDtos = new();

            foreach (var c in creators)
            {
                CreatorExportDto dto = new()
                {
                    CreatorName = c.Name,
                    BoardgamesCount = c.BoardgamesCount
                };
                foreach (var bg in c.Boardgames)
                {
                    BoardgameExportDto bgDto = new()
                    {
                        Name = bg.BoardgameName,
                        YearPublished = bg.BoardgameYearPublished
                    };
                    dto.Boardgames.Add(bgDto);
                }
                creatorExportDtos.Add(dto);
            }

            XmlHelper xmlHelper = new();
            string outputXml = xmlHelper.Serialize(creatorExportDtos, "Creators");
            return outputXml;
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .AsNoTracking()
                .Where(x => x.BoardgamesSellers
                .Any(y => y.Boardgame.YearPublished >= year && y.Boardgame.Rating <= rating))
                .Select(e => new
                {
                    Name = e.Name,
                    Website = e.Website,
                    Boardgames = e.BoardgamesSellers
                    .Where(x => x.Boardgame.YearPublished >= year && x.Boardgame.Rating <= rating)
                    .Select(b => new
                    {
                        Name = b.Boardgame.Name,
                        Rating = b.Boardgame.Rating,
                        Mechanics = b.Boardgame.Mechanics,
                        Category = b.Boardgame.CategoryType.ToString()
                    })
                    .OrderByDescending(r => r.Rating)
                    .ThenBy(n => n.Name)
                    .ToArray()
                })
                .OrderByDescending(c => c.Boardgames.Count())
                .ThenBy(n => n.Name)
                .Take(5)
                .ToArray();


            string outputJson = JsonConvert.SerializeObject(sellers, Formatting.Indented);
            return outputJson;
        }
    }
}