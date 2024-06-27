using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogDemo.Models
{
    //Mark the class with [Table], than give table name "Blogs" and Schema name "blg"
    //Remove builder.ToTable("Blogs", "blg"); from BlogConfiguration ==>
    /*
    ! = from Nullable to Non-Nullable
    ? = from Non-Nullable to Nullable 
    */
    [Table("Blogs", Schema = "blg")]
    [Index(nameof(Name), IsUnique = true, Name = "ix_Blogs_Name_Unique")]

    public class Blog
    {
        [Key] // Identical to builder.HasKey(b => b.BlogId);
        public int BlogId { get; set; }

        //builder.Property(b => b.Name)
        //       .HasColumnName("BlogName")
        //       .HasColumnType("NVARCHAR")
        //       .HasMaxLength(50)
        //       .IsRequired();
        // IS EQUAL TO
        [Required]
        [MaxLength(50)]
        [Column("BlogName", TypeName = "NVARCHAR")]
        public string Name { get; set; } = null!;

        //Write ? to make description property  nullable if <Nullable>enable</Nullable>
        [MaxLength(500)]
        [Column("Blog Description", TypeName = "NVARCHAR")]
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdate { get; set; }

        public int AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; } = null!;
        public List<Post> Posts { get; set; } = new List<Post>();

    }
}
