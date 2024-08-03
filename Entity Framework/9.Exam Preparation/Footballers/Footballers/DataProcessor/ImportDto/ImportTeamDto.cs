using Footballers.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Footballers.Data.DataConstraints;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [Required]
        [MinLength(TeamNameMinValue)]
        [MaxLength(TeamNameMaxValue)]
        [RegularExpression(@"^[a-zA-Z0-9 .-]*$")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(TeamNationalityMinValue)]
        [MaxLength(TeamNationalityMaxValue)]
        public string Nationality { get; set; } = null!;

        [Required]
        public string Trophies { get; set; } = null!;

        [Required]
        public int[] Footballers { get; set; }
    }
}
