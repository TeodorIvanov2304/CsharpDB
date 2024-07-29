using Newtonsoft.Json;

namespace Boardgames.DataProcessor.ExportDto
{
    public class SellerWithMostBoardgamesExportDto
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public  BoardgameExportDto[] Boardgames { get; set; }
    }
}
