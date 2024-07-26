using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Invoices.Data.DataConstraints;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType(nameof(Client))]
    public class ImportClientDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(ClientNameMinLength)]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;
        [XmlElement(nameof(NumberVat))]
        [Required]
        [MinLength(ClientNumberVatMinLength)]
        [MaxLength(ClientNumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        [XmlArray(nameof(Addresses))] 
        public AddressImportDto[] Addresses { get; set; } = null!;
    }
}
