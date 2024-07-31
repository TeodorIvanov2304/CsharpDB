using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class PropertyExportDto
    {
        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier { get; set; } = null!;


        [Required]
        [Range(0,int.MaxValue)]
        public int Area { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; } = null!;

        //string?
        [Required]
        public string DateOfAcquisition { get; set; }

        [Required]
        public OwnerExportDto[] Owners { get; set; }
    }
}
