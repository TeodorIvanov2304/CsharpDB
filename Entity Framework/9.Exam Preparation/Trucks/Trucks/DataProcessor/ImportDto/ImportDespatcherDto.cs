using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Trucks.Data.DataConstraints;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(DespatcherNameMinLength)]
        [MaxLength(DespatcherNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlArray("Trucks")]
        public DispatcherTruckImportDto[] Trucks { get; set; }
    }
}
