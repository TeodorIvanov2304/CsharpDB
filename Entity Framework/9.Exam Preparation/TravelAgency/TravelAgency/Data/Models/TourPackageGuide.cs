using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgency.Data.Models
{
    public class TourPackageGuide
    {
        [Key]
        [ForeignKey(nameof(TourPackage))]
        public int TourPackageId  { get; set; }
        public TourPackage TourPackage { get; set; }

        [Key]
        [ForeignKey(nameof(Guide))]
        public int GuideId  { get; set; }
        public Guide Guide { get; set; }
    }
}
