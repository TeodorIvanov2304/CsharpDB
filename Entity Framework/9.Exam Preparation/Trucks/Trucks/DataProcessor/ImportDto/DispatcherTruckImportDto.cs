using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;
using static Trucks.Data.DataConstraints;
namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class DispatcherTruckImportDto
    {
        [Required]
        [XmlElement("RegistrationNumber")]
        [MaxLength(8)]
        [RegularExpression(@"^[A-Z]{2}[0-9]{4}[A-Z]{2}\b")]
        public string RegistrationNumber { get; set; } = null!;

        [Required]
        [XmlElement("VinNumber")]
        [MinLength(TruckVinNumberLength)]
        [MaxLength(TruckVinNumberLength)]
        public string VinNumber { get; set; } = null!;

        [Required]
        [XmlElement("TankCapacity")]
        [Range(TruckTankCapacityMinLength,TruckTankCapacityMaxLength)]
        public int TankCapacity { get; set; }

        [Required]
        [XmlElement("CargoCapacity")]
        [Range(TruckCargoCapacityMinLength,TruckCargoCapacityMaxLength)]
        public int CargoCapacity { get; set; }

        [Required]
        [XmlElement("CategoryType")]
        [Range(TruckCategoryTypeMinValue,TruckCategoryTypeMaxValue)]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement("MakeType")]
        [Range(TruckMakeTypeMinValue,TruckMakeTypeMaxValue)]
        public int MakeType { get; set; }
    }
}
