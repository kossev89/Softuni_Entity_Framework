namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ImportDto;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Globalization;
    using Footballers.Utilities;
    using Castle.Core.Internal;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System.Globalization;
    using Footballers.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder result = new();
            List<Coach> coachesToImport = new();
            string rootName = "Coaches";
            CoachImportXmlDto[] coachesDtos = XmlSerializationHelper.Deserialize<CoachImportXmlDto[]>(xmlString, rootName);

            foreach (var c in coachesDtos)
            {
                if (!IsValid(c)
                    || c.Nationality.IsNullOrEmpty()
                    )
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }
                Coach coach = new()
                {
                    Name = c.Name,
                    Nationality = c.Nationality
                };

                foreach (var f in c.Footballers)
                {
                    DateTime ContractStartDate;
                    DateTime ContractEndDate;
                    string format = "dd/MM/yyyy";
                    if (!DateTime.TryParseExact(f.ContractStartDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ContractStartDate)
                        || !DateTime.TryParseExact(f.ContractEndDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ContractEndDate)
                        )
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (!IsValid(f)
                        || ContractStartDate > ContractEndDate
                        )
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }
                    Footballer footballer = new()
                    {
                        Name = f.Name,
                        ContractStartDate = ContractStartDate,
                        ContractEndDate = ContractEndDate,
                        BestSkillType = (BestSkillType)f.BestSkillType,
                        PositionType = (PositionType)f.PositionType
                    };
                    coach.Footballers.Add(footballer);
                }

                coachesToImport.Add(coach);
                result.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count()));
            }

            context.Coaches.AddRange(coachesToImport);
            context.SaveChanges();
            return result.ToString().Trim();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder result = new();
            List<Team> teams = new();
            TeamImportJsonDto[] teamsDtos = JsonConvert.DeserializeObject<TeamImportJsonDto[]>(jsonString);

            foreach (var t in teamsDtos)
            {
                if (!IsValid(t)
                    || t.Trophies < 1
                    )
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }
                Team team = new()
                {
                    Name = t.Name,
                    Nationality = t.Nationality,
                    Trophies = t.Trophies
                };

                foreach (var id in t.Footballers)
                {
                    if (!context.Footballers.Any(x => x.Id == id))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }
                    TeamFootballer teamFootballer = new()
                    {
                        TeamId = team.Id,
                        FootballerId = id
                    };
                    team.TeamsFootballers.Add(teamFootballer);
                }

                teams.Add(team);
                result.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count()));
            }

            context.Teams.AddRange(teams);
            context.SaveChanges();
            return result.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
