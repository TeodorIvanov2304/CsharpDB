using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirstDemoo.Infrastructure.Data.Models
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        //NVARCHAR(50) NOT NULL
        [Required]
        [StringLength(50)]
        [Unicode(true)]
        public string Name { get; set; } = null!; //Null-neglating operator

    }
}
