using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Cadastre.Common.ValidationConstants;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictDto
    {
        
        [XmlAttribute("Region")]
        [Required]
        public Region Region { get; set; }

        [XmlElement("Name")]
        [Required]
        [MinLength(DistrictNameMinLength)]
        [MaxLength(DistrictNameMaxLength)]
        public string Name { get; set; }

        [XmlElement("PostalCode")]
        [Required]
        [RegularExpression("^[A-Z]{2}-[0-9]{5}$")]
        public string PotalCode { get; set; }

        [XmlArray("Properties")]
        public ImportPropertyDto[] Properties { get; set; }
    }
}
