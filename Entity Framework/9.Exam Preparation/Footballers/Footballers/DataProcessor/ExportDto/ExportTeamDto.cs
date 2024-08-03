namespace Footballers.DataProcessor.ExportDto
{
    public class ExportTeamDto
    {
        public string Name { get; set; }
        public ExportTeamFootballersDto[] Footballers { get; set; }
    }
}
