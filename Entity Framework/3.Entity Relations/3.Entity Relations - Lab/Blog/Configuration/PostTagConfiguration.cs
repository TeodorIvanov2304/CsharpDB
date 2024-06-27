using BlogDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogDemo.Configuration
{
    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {   
        //Composite PK for PostTag
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.HasKey(e => new { e.PostId, e.TagId });
        }
    }
}
