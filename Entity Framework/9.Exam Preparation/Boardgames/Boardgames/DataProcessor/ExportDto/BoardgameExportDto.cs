using Boardgames.Data.Models.Enums;

namespace Boardgames.DataProcessor.ExportDto
{
    public class BoardgameExportDto
    {
        public string Name { get; set; }
        public double Rating { get; set; }
        public string Mechanics { get; set; }
        public string Category { get; set; }
    }
}