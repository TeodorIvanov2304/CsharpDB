using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("buyer")]
    public class BuyerInRangeExportDto
    {
        [XmlElement()]
        public string FullName { get; set; }
    }
}