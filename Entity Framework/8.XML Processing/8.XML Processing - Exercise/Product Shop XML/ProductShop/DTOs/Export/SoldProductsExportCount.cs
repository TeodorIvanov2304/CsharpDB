using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]
    public class SoldProductsExportCount
    {
        [XmlElement("count")]
        public int Count { get; set; }
        [XmlArray("users")]
        [XmlArrayItem("User")]
        public List<UserWith1SoldProductDto> Users { get; set; }
    }
}