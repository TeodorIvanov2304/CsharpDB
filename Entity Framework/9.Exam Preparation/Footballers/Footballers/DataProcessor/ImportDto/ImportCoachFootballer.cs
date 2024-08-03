using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Footballers.Data.DataConstraints;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportCoachFootballer
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(FootballerNameMinValue)]
        [MaxLength(FootballerNameMaxValue)]
        public string Name { get; set; } = null!;

        
        [Required]
        [XmlElement("ContractStartDate")]
        public string ContractStartDate { get; set; }

        [Required]
        [XmlElement("ContractEndDate")]
        public string ContractEndDate { get; set; }

        [Required]
        [XmlElement("BestSkillType")]
        [Range(FootballerBestSkillTypeMinValue,FootballerBestSkillTypeMaxValue)]
        public int BestSkillType { get; set; }

        [Required]
        [XmlElement("PositionType")]
        [Range(FootballerPositionTypeMinValue,FootballerPositionTypeMaxValue)]
        public int PositionType { get; set; }
    }
}
