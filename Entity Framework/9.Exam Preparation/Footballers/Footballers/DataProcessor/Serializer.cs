﻿namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .Where(c=>c.Footballers.Any())
                .OrderByDescending(c=>c.Footballers.Count)
                .ThenBy(c=>c.Name)
                .Select(c => new ExportCoachDto()
                {
                    FootballersCount = c.Footballers.Count,
                    CoachName = c.Name,
                    Footballers = c.Footballers
                    .Select(f => new ExportCoachFootballer()
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .OrderBy(f=>f.Name)
                    .ToArray()
                })
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();
            const string root = "Coaches";
            return xmlHelper.Serialize(coaches, root);
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .Select(t => new ExportTeamDto()
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                    .OrderByDescending(tf=>tf.Footballer.ContractEndDate)
                    .ThenBy(tf=>tf.Footballer.Name)
                    .Select(tf => new ExportTeamFootballersDto()
                    {
                        FootballerName = tf.Footballer.Name,
                        ContractStartDate = tf.Footballer.ContractStartDate.ToString                  ("d",CultureInfo.InvariantCulture),
                        ContractEndDate = tf.Footballer.ContractEndDate.ToString("d",CultureInfo.InvariantCulture),
                        BestSkillType = tf.Footballer.BestSkillType.ToString(),
                        PositionType = tf.Footballer.PositionType.ToString()
                    })            
                    .ToArray()
                })
                .OrderByDescending(t=>t.Footballers.Length)
                .ThenBy(t=>t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
