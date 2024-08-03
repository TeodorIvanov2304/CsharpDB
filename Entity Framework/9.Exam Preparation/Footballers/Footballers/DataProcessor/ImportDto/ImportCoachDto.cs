using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Footballers.Data.DataConstraints;

namespace Footballers.DataProcessor.ImportDto

{
    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(CoachNameMinValue)]
        [MaxLength(CoachNameMaxValue)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("Nationality")]
        public string Nationality { get; set; } = null!;

        [XmlArray("Footballers")]
        public ImportCoachFootballer[] Footballers { get; set; }
    }
}
