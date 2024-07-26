using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Invoices.Data.DataConstraints;
namespace Invoices.Data.Models
{
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required] // decimal is required by default
        public decimal Price { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; } //Enumeration in DB is stored as INT --> by default REQUIRED

        //TODO: Add navigation property
        public virtual ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();
    }
}
