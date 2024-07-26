using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]   
    
    public class SellerWithProductsExoportDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }
        [XmlElement("lastName")]
        public string LastName { get; set; }
        [XmlArray("soldProducts")]
        public List<SoldProductsExportDto> SoldProducts { get; set; }
    }
}