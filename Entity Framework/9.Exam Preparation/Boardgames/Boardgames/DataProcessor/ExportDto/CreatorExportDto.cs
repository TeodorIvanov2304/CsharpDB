using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class CreatorExportDto
    {
        [XmlElement("CreatorName")]
        public string CreatorName { get; set; }

        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        [XmlArray("Boardgames")]
        public CreatorBoardGameExportDto[] Boardgames { get; set; }
    }
}
