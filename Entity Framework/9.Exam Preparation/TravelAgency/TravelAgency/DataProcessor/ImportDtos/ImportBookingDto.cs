using System.ComponentModel.DataAnnotations;
using static TravelAgency.Data.DataConstraints;
namespace TravelAgency.DataProcessor.ImportDtos
{
    public class ImportBookingDto
    {
        [Required]
        public string BookingDate { get; set; } = null!;

        [Required]
        [MinLength(CustomerFullNameMinValue)]
        [MaxLength(CustomerFullNameMaxValue)]
        public string CustomerName { get; set; } = null!;

        [Required]
        [MinLength(TourPackageNameMinValue)]
        [MaxLength(TourPackageNameMaxValue)]
        public string TourPackageName { get; set; } = null!;
    }
}
