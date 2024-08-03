using System.ComponentModel.DataAnnotations;
using static Footballers.Data.DataConstraints;

namespace Footballers.Data.Models
{
    public class Coach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CoachNameMaxValue)]
        public string Name { get; set; } = null!;

        [Required]
        public string Nationality { get; set; } = null!;
        public ICollection<Footballer> Footballers  { get; set; } = new List<Footballer>();
    }
}
