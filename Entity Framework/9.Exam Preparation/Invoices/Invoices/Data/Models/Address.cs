using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Invoices.Data.DataConstraints;

namespace Invoices.Data.Models
{
    public class Address
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(AddressStreetNameMaxLength)]
        public string StreetName  { get; set; } = null!;

        [Required] // by default required
        public int StreetNumber  { get; set; }

        [Required]
        public string PostCode  { get; set; } = null!;  //VARCHAR(MAX)

        [Required]
        [MaxLength(CityNameMaxLength)]
        public string City  { get; set; } = null!;

        [Required]
        [MaxLength(CountryNameMaxLength)]
        public string Country { get; set; } = null!;

        //TODO: Add navigation property
        [Required]
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        [Required]
        public virtual Client Client { get; set; } = null!;
    }
}
