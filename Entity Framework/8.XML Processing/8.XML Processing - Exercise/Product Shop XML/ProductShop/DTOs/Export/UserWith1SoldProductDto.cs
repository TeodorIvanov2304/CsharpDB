using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]
    public class UserWith1SoldProductDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }
        [XmlElement("lastName")]
        public string LastName { get; set; }
        [XmlElement("age")]
        public int? Age { get; set; }
        public int Count { get; set; }
        [XmlArray("SoldProducts")]
        public List<SoldProductsExportDto> SoldProducts { get; set; }

    }
}