using System.ComponentModel.DataAnnotations;
using static TravelAgency.Data.DataConstraints;

namespace TravelAgency.Data.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CustomerFullNameMaxValue)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(CustomerEmailMaxValue)]
        public string Email { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; }
    }
}
