using Medicines.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Medicines.Data.DataConstraints;
namespace Medicines.DataProcessor.ImportDtos
{
    public class PatientImportDto
    {
        [Required]
        [MinLength(PatientFullNameMinValue)]
        [MaxLength(PatientFullNameMaxValue)]
        public string FullName { get; set; } = null!;
        [Required]
        [Range(0,2)]
        public int AgeGroup { get; set; }
        [Required]
        [Range(0,1)]
        public int Gender { get; set; }
        [Required]
        public int[] Medicines { get; set; }
    }
}
