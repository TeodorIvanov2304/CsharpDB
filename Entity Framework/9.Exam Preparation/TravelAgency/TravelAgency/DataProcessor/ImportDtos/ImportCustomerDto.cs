using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static TravelAgency.Data.DataConstraints;

namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [Required]
        [XmlAttribute("phoneNumber")]
        [RegularExpression(@"^\+[0-9]{12}$")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [XmlElement("FullName")]
        [MinLength(CustomerFullNameMinValue)]
        [MaxLength(CustomerFullNameMaxValue)]
        public string FullName { get; set; } = null!;

        [Required]
        [XmlElement("Email")]
        [MinLength(CustomerEmailMinValue)]
        [MaxLength(CustomerEmailMaxValue)]
        public string Email { get; set; } = null!;
    }
}
