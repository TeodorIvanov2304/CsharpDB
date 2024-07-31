using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Cadastre.Common.ValidationConstants;
namespace Cadastre.DataProcessor.ImportDtos
{
    public class ImportCitizenDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(CitizenNameMinLength)]
        [MaxLength(CitizenNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        public string BirthDate { get; set; } = null!;

        [Required]
        public string MaritalStatus { get; set; } = null!;
        [Required]
        public int[] Properties { get; set; }
    }
}
