namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using Trucks.DataProcessor.ExportDto;
    using Trucks.Utilities;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatchersWithTrucks = context.Despatchers
                .Where(d=>d.Trucks.Any())
                .Select(d => new DespetcherExportDto()
                {
                    TrucksCount = d.Trucks.Count,
                    DespatcherName = d.Name,
                    Trucks = d.Trucks
                    .OrderBy(t=>t.RegistrationNumber)
                    .Select(t => new DespethcerExportTruckDto()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString()
                    })
                    .ToArray()
                })
                .OrderByDescending(d=>d.TrucksCount)
                .ThenBy(d=>d.DespatcherName)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Despatchers";
            return xmlHelper.Serialize(despatchersWithTrucks, xmlRoot);
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clientsWithTrucks = context.Clients
                .Where(c => c.ClientsTrucks.Any() && c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .Select(c => new ClientExportDto()
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                    .Where(t=>t.Truck.TankCapacity >= capacity)
                    .OrderBy(t => t.Truck.MakeType)
                    .ThenByDescending(t=>t.Truck.CargoCapacity)
                    .Select(t => new ExportTruckDto()
                    {
                        TruckRegistrationNumber = t.Truck.RegistrationNumber,
                        VinNumber = t.Truck.VinNumber,
                        TankCapacity = t.Truck.TankCapacity,
                        CargoCapacity = t.Truck.CargoCapacity,
                        CategoryType = t.Truck.CategoryType.ToString(),
                        MakeType = t.Truck.MakeType.ToString()
                    })
                    .ToArray()

                })
                .OrderByDescending(t=>t.Trucks.Length)
                .ThenBy(t=>t.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clientsWithTrucks,Formatting.Indented);
        }
    }
}
