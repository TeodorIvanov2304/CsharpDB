using BlogDemo.Configuration;
using BlogDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlogDemo
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {
                
        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options) 
        {
            
        }

        //DbSets == SQL tables
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }


        //Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Moved the method implementation in Configuration/BlogConfiguration/Configure

            //Apply the already moved configuration with new instance of BlogConfiguration
            modelBuilder.ApplyConfiguration(new BlogConfiguration());

            //Apply the PostTag configuration
            modelBuilder.ApplyConfiguration(new PostTagConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate = true;";
             optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }
}
