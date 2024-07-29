using Newtonsoft.Json;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using static Boardgames.Data.DataConstraints;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellerDto
    {
        

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Name { get; set; } 

        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Address { get; set; } 

        [Required]
        public string Country { get; set; } 

        [Required]
        [RegularExpression(@"www.[a-zA-Z\d*-]+.com")]
        public string Website { get; set; } = null!;

        public int[] Boardgames { get; set; }

    }
}