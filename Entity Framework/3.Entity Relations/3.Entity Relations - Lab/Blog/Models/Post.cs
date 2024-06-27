//ONE-TO-MANY-RELATIONSHIP


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace BlogDemo.Models
{
    [Table("Posts",Schema = "blg")]
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;
        [Required]
        //No max length means MAX length
        [Column("Content", TypeName = "NVARCHAR")]
        public string Content { get; set; } = null!;
        public int BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; }

        //Property for the composite PK
        public List<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
