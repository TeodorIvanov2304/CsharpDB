using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogDemo.Models
{
    [Table("Tags",Schema = "blg")]
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required]
        [MaxLength(15)]
        public string TagName { get; set; } = null!;

        //Property for the composite PK
        public List<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
