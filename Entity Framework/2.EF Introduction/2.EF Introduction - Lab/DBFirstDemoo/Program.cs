
//Package manager command
//Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=SoftUni;Trusted_Connection=True;TrustServerCertificate = true;" Microsoft.EntityFrameworkCore.SqlServer -DataAnnotations -Context SoftUniDbContext -ContextDir Data -OutputDir Data/Models



using DBFirstDemoo.Infrastructure.Data;
using DBFirstDemoo.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;


using SoftUniDbContext softUniDbContext = new SoftUniDbContext();
var employees = await softUniDbContext.Employees.Select(e => new
{
    e.FirstName,
    e.LastName,
    e.HireDate,
    e.Projects
}).Where(e => e.FirstName == "Guy" ).ToListAsync(); ;


//Generates the SQL query
//var employees2 =  softUniDbContext.Employees.Select(e => new
//{
//    e.FirstName,
//    e.LastName,
//    e.HireDate,
//    e.Projects
//}).Where(e => e.LastName == "Walters").ToQueryString();

var project = softUniDbContext.Projects.Find(2);

Console.WriteLine($"{project.Name} {project.ProjectId}");

Employee employee = new Employee()
{
    FirstName = "Petar",
    LastName = "Petrov",
    DepartmentId = 1,
    HireDate = DateTime.UtcNow,
    JobTitle = "Test",
    Salary = 12000
};

await softUniDbContext.Employees.AddAsync(employee);
var employee1 = await softUniDbContext.Employees
                      .Where(e => e.LastName == "Kulov")
                      .FirstOrDefaultAsync();

//Change specific employee Salary 
employee1.Salary = 0;

//Save changes
await softUniDbContext.SaveChangesAsync();

//Remove employee1 and then save changes
softUniDbContext.Employees.Remove(employee1);

await softUniDbContext.SaveChangesAsync();

