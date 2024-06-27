using BlogDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace BlogDemo.Configuration
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {

            //After moving from BlogDbContext replace modelBuilder.Entity<Blog>() with builder


            //PK - the property BlogId

            //Remove the row below
            //builder.HasKey(b => b.BlogId);

            //Create table with name blog
            //builder.ToTable("Blogs", "blg");

            //Fluent API
            //Setting the "Name" property IsRequired = NOT NULL
            //builder.Property(b => b.Name)
            //       .HasColumnName("BlogName")
            //       .HasColumnType("NVARCHAR")
            //       .HasMaxLength(50)
            //       .IsRequired();

            // Create the table with update-database command in PMC

            //Setting the "Description" property
            builder.Property(b => b.Description)
                   .HasColumnName("Blog Description")
                   .HasColumnType("NVARCHAR")
                   .HasMaxLength(500);

            //Setting the "LastUpdated" property
            builder.Property(b => b.LastUpdate)
                   .ValueGeneratedOnUpdate(); //Generate DateTime on update

            //Setting the "Created" property
            builder.Property(b => b.Created)
                   .ValueGeneratedOnAdd();         //Generate DateTime on add

            //After that we create the public DbSet<Blog> Blogs {get; set;} property
            
        }
    }
}
