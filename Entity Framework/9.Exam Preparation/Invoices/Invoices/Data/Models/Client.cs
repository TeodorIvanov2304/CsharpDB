using System.ComponentModel.DataAnnotations;
using static Invoices.Data.DataConstraints;

namespace Invoices.Data.Models
{
    public class Client
    {
        [Key]   
        public int Id { get; set; }

        [Required]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ClientNameMaxLength)]
        public string NumberVat { get; set; } = null!;

        //TODO:Navigation properties

        public virtual ICollection<Address> Addresses  { get; set; } = new HashSet<Address>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
        public virtual ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();
    }
}
