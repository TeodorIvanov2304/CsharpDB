using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
       private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=StudentSystem;Trusted_Connection=True;TrustServerCertificate = true;";

        public StudentSystemContext(DbContextOptions options) : base(options)
        {
            
        }

        //Add contexts
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentsCourses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> Homeworks { get; set; }

        //Sets the DB in use
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(ConnectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            //Configure the composite PK
            modelBuilder.Entity<StudentCourse>()
                        .HasKey(sc => new { sc.StudentId, sc.CourseId });

            //Make PhoneNumber not unicode
            //modelBuilder.Entity<Student>()
            //            .Property(s => s.PhoneNumber)
            //            .IsUnicode(false); // == [Unicode(false)]


        }
    }
}
