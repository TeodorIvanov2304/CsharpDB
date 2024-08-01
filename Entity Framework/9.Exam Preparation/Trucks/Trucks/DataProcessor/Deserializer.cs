namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new XmlHelper();
            const string root = "Despatchers";
            ImportDespatcherDto[] importDespatchers = xmlHelper.Deserialize<ImportDespatcherDto[]>(xmlString, root);
            ICollection<Despatcher> decpathcersToImport = new List<Despatcher>();

            foreach (var despatcherDto in importDespatchers)
            {
                if (!IsValid(despatcherDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (despatcherDto == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Truck> trucksToImport = new List<Truck>();

                foreach (var truckDto in despatcherDto.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    Truck newTruck = new Truck()
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = (CategoryType)truckDto.CategoryType,
                        MakeType = (MakeType)truckDto.MakeType
                    };

                    trucksToImport.Add(newTruck);
                }

                Despatcher newDespatcher = new Despatcher()
                {
                    Name = despatcherDto.Name,
                    Position = despatcherDto.Position,
                    Trucks = trucksToImport
                };

                decpathcersToImport.Add(newDespatcher);
                sb.AppendLine(String.Format(SuccessfullyImportedDespatcher, newDespatcher.Name, newDespatcher.Trucks.Count));
            }


            context.Despatchers.AddRange(decpathcersToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportClientDto[] clientDtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);

            List<Client> clients = new List<Client>();

            foreach (ImportClientDto clientDto in clientDtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client c = new Client()
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type,
                    ClientsTrucks = new List<ClientTruck>()
                };

                if (c.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                foreach (int truckId in clientDto.Trucks.Distinct())
                {
                    Truck t = context.Trucks.Find(truckId);
                    if (t == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    c.ClientsTrucks.Add(new ClientTruck()
                    {
                        Truck = t
                    });
                }
                clients.Add(c);
                sb.AppendLine(String.Format(SuccessfullyImportedClient, c.Name, c.ClientsTrucks.Count));
            }
            context.Clients.AddRange(clients);
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