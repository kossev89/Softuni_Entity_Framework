namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coachesFiltered = context.Coaches
                .Where(c => c.Footballers.Any())
                .Select(e => new
                {
                    e.Name,
                    Footballers = e.Footballers
                    .Select(f => new
                    {
                        f.Name,
                        f.PositionType
                    })
                    .OrderBy(n => n.Name)
                    .ToArray()
                })
                .OrderByDescending(c => c.Footballers.Count())
                .ThenBy(n => n.Name)
                .ToArray();

            List<CoachExportXmlDto> coachExports = new();

            foreach (var c in coachesFiltered)
            {
                CoachExportXmlDto coach = new()
                {
                    CoachName = c.Name,
                    FootballersCount = c.Footballers.Count()
                };

                foreach (var f in c.Footballers)
                {
                    FootbalerExportXmlDto footbaler = new()
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    };
                    coach.Footballers.Add(footbaler);
                }

                coachExports.Add(coach);
            }

            string rootName = "Coaches";
            return XmlSerializationHelper.Serialize(coachExports, rootName);
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            List<TeamExportJsonDto> teamsToExport = new();
            var teamsFiltered = context.Teams
                .Where(x => x.TeamsFootballers.Any(y => y.Footballer.ContractStartDate >= date))
                .Select(e => new
                {
                    e.Name,
                    Footballers = e.TeamsFootballers
                    .Where(x => x.Footballer.ContractStartDate >= date)
                    .Select(f => new
                    {
                        FootballerName = f.Footballer.Name,
                        f.Footballer.ContractStartDate,
                        f.Footballer.ContractEndDate,
                        f.Footballer.BestSkillType,
                        f.Footballer.PositionType
                    })
                    .OrderByDescending(e => e.ContractEndDate)
                    .ThenBy(n => n.FootballerName)
                    .ToArray()
                })
                .OrderByDescending(c => c.Footballers.Count())
                .ThenBy(n => n.Name)
                .Take(5)
                .ToArray();

            foreach (var t in teamsFiltered)
            {
                TeamExportJsonDto team = new()
                {
                    Name = t.Name
                };

                foreach (var f in t.Footballers)
                {

                    FootballerExportJsonDto footballer = new()
                    {
                        FootballerName = f.FootballerName,
                        ContractStartDate = f.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.BestSkillType.ToString(),
                        PositionType = f.PositionType.ToString()
                    };
                    team.Footballers.Add(footballer);
                }

                teamsToExport.Add(team);
            }

            return JsonConvert.SerializeObject(teamsToExport, Formatting.Indented);
        }
    }
}

