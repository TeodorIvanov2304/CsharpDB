using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Medicines.Data.DataConstraints;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]   
    public class MedicineImportDto
    {
        [Required]         
        [XmlAttribute("category")]
        [Range(MedicineCategoryMinValue, MedicineCategoryMaxValue)]
        public int Category { get; set; }

        [Required]
        [XmlElement("Name")]
        [MinLength(MedicineNameMinValue)]
        [MaxLength(MedicineNameMaxValue)]
        public string Name { get; set; } = null!;
        
        [Required]
        [XmlElement("Price")]
        [Range(typeof(decimal), MedicinePriceMinValue, MedicinePriceMaxValue)]
        public decimal Price { get; set; }

        [Required]
        [XmlElement("ProductionDate")]
        public string ProductionDate { get; set; }

        [Required]
        [XmlElement("ExpiryDate")]
        public string ExpiryDate { get; set; }

        [Required]
        [MinLength(MedicineProducerMinValue)]
        [MaxLength(MedicineProducerMaxValue)]
        public string Producer { get; set; } = null!;
    }
}
