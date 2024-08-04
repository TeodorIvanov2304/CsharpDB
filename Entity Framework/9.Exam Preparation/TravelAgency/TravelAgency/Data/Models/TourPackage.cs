using System.ComponentModel.DataAnnotations;
using static TravelAgency.Data.DataConstraints;

namespace TravelAgency.Data.Models
{
    public class TourPackage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TourPackageNameMaxValue)]
        public string PackageName { get; set; } = null!;

        [MaxLength(TourPackageDescriptionMaxValue)]
        public string? Description  { get; set; }

        [Required]
        public decimal Price  { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

        public ICollection<TourPackageGuide> TourPackagesGuides { get; set; } = new HashSet<TourPackageGuide>();
    }
}
