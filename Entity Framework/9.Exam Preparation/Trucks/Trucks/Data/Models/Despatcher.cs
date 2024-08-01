using System.ComponentModel.DataAnnotations;
using static Trucks.Data.DataConstraints;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(DespatcherNameMaxLength)]
        public string Name { get; set; } = null!;
        public string Position { get; set; }
        public ICollection<Truck> Trucks { get; set; }
    }
}
