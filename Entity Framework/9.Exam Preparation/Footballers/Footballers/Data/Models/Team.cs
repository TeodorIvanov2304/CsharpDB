using System.ComponentModel.DataAnnotations;
using static Footballers.Data.DataConstraints;

namespace Footballers.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TeamNameMaxValue)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(TeamNationalityMaxValue)]
        public string Nationality  { get; set; } = null!;

        [Required]
        public int Trophies  { get; set; }
        public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new List<TeamFootballer>();
    }
}
