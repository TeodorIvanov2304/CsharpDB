namespace Trucks.DataProcessor.ExportDto
{
    public class ClientExportDto
    {
        public string Name { get; set; }
        public ExportTruckDto[] Trucks { get; set; }
    }
}
