using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//Add-Migration AuthorAdded
//Update-Database
/*
  ! = from Nullable to Non-Nullable
  ? = from Non-Nullable to Nullable 
  */

//ONE-TO-ONE-OR-ZERO RELATION

namespace BlogDemo.Models
{
    [Table("Authors", Schema = "blg")]
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Required]
        [MaxLength(100)]
        [Column("AuthorName", TypeName = "NVARCHAR")]
        public string AuthorName { get; set; } = null!; //Musta have a name!! therefore null!;
        public int? BlogId { get; set; }                //Its not required, so we type ? for not nullable
        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; } = null!;        //Its not required, there can be no blog, so again ?


    }
}
