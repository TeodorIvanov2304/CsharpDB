using System.ComponentModel.DataAnnotations.Schema;


//MAPPING TABLE MANY-TO-MANY

namespace BlogDemo.Models
{
    [Table("PostsTags",Schema = "blg")]
    public class PostTag
    {
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;
        public int TagId { get; set; }

        [ForeignKey(nameof (TagId))]
        public Tag Tag { get; set; } = null!;
    }
}
