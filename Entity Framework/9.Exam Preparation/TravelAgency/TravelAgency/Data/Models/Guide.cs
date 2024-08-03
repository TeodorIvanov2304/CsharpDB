using System.ComponentModel.DataAnnotations;
using TravelAgency.Data.Models.Enums;
using static TravelAgency.Data.DataConstraints;

namespace TravelAgency.Data.Models
{
    public class Guide
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GuideFullNameMaxValue)]
        public string FullName { get; set; } = null!;

        [Required]
        public Language Language  { get; set; }

        public ICollection<TourPackageGuide> TourPackagesGuides  { get; set; }
    }
}
