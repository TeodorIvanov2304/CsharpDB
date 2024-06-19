using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDEMO
{
    public class SoftUniDbContext : DbContext
    {
        public SoftUniDbContext()
        {

        }

        public SoftUniDbContext(DbContextOptions<SoftUniDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SoftUni;Trusted_Connection=True;TrustServerCertificate = true";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
