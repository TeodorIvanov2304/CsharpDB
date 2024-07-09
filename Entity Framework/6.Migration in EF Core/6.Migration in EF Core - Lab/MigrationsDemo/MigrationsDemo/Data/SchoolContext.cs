using Microsoft.EntityFrameworkCore;
using MigrationsDemo.Models;

namespace MigrationsDemo.Data
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {   

            //Use 2 DBs
            if (Environment.GetEnvironmentVariable("DATABASE_PROVIDER") == "PostgreSql")
            {
                optionsBuilder.UseNpgsql("Host=(localdb)\\MSSQLLocalDB;Database=School_Db;User Id=postgres;Password:postgres;").UseSnakeCaseNamingConvention();
            }
            else
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=School;Trusted_Connection=True;");
            }
            
        }
    }
}
