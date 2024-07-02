using System.ComponentModel.DataAnnotations;

namespace DBFirstDemo
{
    //DTO Data Transfer Object
    public class EmployeeDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; }  = null!;
        public string JobTitle { get; set; } = null!;
    }
}
