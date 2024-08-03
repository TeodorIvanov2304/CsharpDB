namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new();
            const string root = "Coaches";

            var coachesDto = xmlHelper.Deserialize<ImportCoachDto[]>(xmlString,root);
            ICollection<Coach> coachesToImport = new List<Coach>();

            foreach ( var coachDto in coachesDto )
            {
                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (String.IsNullOrEmpty(coachDto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach newCoach = new Coach() 
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality,
                    Footballers = new List<Footballer>()
                };

                foreach (var footballerDto in coachDto.Footballers)
                {
                    if (!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime contractStartDate = DateTime.ParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime contractEndDate = DateTime.ParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy",CultureInfo.InvariantCulture);

                    if (DateTime.Compare(contractStartDate, contractEndDate) > 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer newFootballer = new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                        PositionType = (PositionType)footballerDto.PositionType
                    };

                    newCoach.Footballers.Add(newFootballer);
                }

                coachesToImport.Add(newCoach);
                sb.AppendLine(String.Format(SuccessfullyImportedCoach, newCoach.Name, newCoach.Footballers.Count));
            }

            context.Coaches.AddRange(coachesToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new();
            var teamsDto = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);
            ICollection<Team> teamsToImport = new List<Team>();

            foreach (var teamDto in teamsDto)
            {
                if (!IsValid(teamDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (int.Parse(teamDto.Trophies) == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team newTeam = new Team() 
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = int.Parse(teamDto.Trophies),
                    TeamsFootballers = new List<TeamFootballer>()
                };

                foreach (var footballerId in teamDto.Footballers.Distinct())
                {
                    Footballer footballer = context.Footballers.Find(footballerId);
                    
                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer newTeamFootballer = new TeamFootballer
                    {
                        FootballerId = footballerId,
                        Team = newTeam
                    };

                    newTeam.TeamsFootballers.Add(newTeamFootballer);
                }

                teamsToImport.Add(newTeam);
                sb.AppendLine(String.Format(SuccessfullyImportedTeam, newTeam.Name, newTeam.TeamsFootballers.Count));

            }

            context.Teams.AddRange(teamsToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
