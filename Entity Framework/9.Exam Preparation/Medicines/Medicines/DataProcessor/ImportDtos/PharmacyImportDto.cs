using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Medicines.Data.DataConstraints;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class PharmacyImportDto
    {
        [Required]
        [XmlAttribute("non-stop")]
        public string IsNonStop { get; set; }

        [Required]
        [XmlElement("Name")]
        [MinLength(PharmacyNameMinLength)]
        [MaxLength(PharmacyNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("PhoneNumber")]
        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}\b")]
        public string PhoneNumber { get; set; } = null!;

        [XmlArray("Medicines")]
        public MedicineImportDto[] Medicines { get; set; }

    }
}
