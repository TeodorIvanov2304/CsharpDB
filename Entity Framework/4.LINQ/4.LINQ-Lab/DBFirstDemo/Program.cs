
//Package manager command
//Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=SoftUni;Trusted_Connection=True;TrustServerCertificate = true;" Microsoft.EntityFrameworkCore.SqlServer -DataAnnotations -Context SoftUniDbContext -ContextDir Data -OutputDir Data/Models



using DBFirstDemo;
using DBFirstDemoo.Infrastructure.Data;
using DBFirstDemoo.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


using SoftUniDbContext softUniDbContext = new SoftUniDbContext();


//EXAMPLES

//Classical LINQ (Expression three)
var employees = from employee in softUniDbContext.Employees
                where employee.DepartmentId == 1
                select employee;

foreach (var employee in employees)
{
    Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.JobTitle}");
}

//New LINQ style
//We use the DTO to transfer data outside the system
var employees2 =  await softUniDbContext.Employees
                           .Where(e => e.DepartmentId == 1)
                           .Select(e => new EmployeeDto()
                           {
                               FirstName = e.FirstName,
                               LastName = e.LastName,
                               JobTitle = e.JobTitle
                           })
                           .ToListAsync();


//Count
var employeeCount = await employees.Where(e => e.DepartmentId == 1)
                                   .CountAsync();

//Average
decimal averageSalary = await softUniDbContext.Employees
                                          .Where(d => d.DepartmentId == 2)
                                          .AverageAsync(e => e.Salary);

//Min
decimal minAverageSalary = await softUniDbContext.Employees
                                          .Where(d => d.DepartmentId == 2)
                                          .MinAsync(e => e.Salary);

//Max
decimal maxAverageSalary = await softUniDbContext.Employees
                                          .Where(d => d.DepartmentId == 2)
                                          .MaxAsync(e => e.Salary);

//Sum
decimal sumSalary = await softUniDbContext.Employees
                                          .Where(d => d.DepartmentId == 2)
                                          .SumAsync(e => e.Salary);

//JOIN
//Join without join
var emp = await softUniDbContext.Employees.Where(e => e.DepartmentId == 1)
                                    .Select(e => new
                                    {
                                        e.FirstName,
                                        e.LastName,
                                        e.JobTitle,
                                        Department = e.Department.Name // JOIN Department

                                    }).ToListAsync();
Console.WriteLine("-------------");
foreach (var item in emp)
{
    Console.WriteLine($"{item.FirstName} {item.LastName} {item.JobTitle} from {item.Department}");
}

//Join with Join() ---> with select
var emp2 = await softUniDbContext.Employees.Where(e => e.DepartmentId ==1)
                                           .Join(softUniDbContext.Departments,
                                                 e => e.DepartmentId,
                                                 d => d.DepartmentId,
                                                 (e, d) => new
                                                 {
                                                     e.FirstName,
                                                     e.LastName,
                                                     e.JobTitle,
                                                     Department = d.Name
                                                 })
                                           .ToListAsync();

//With Include() ----> without select : *
var emp3 = await softUniDbContext.Employees.Where(e => e.DepartmentId == 1)
                                           .Include(e => e.Department)
                                           .ToListAsync();


//GroupBy() example

var emp4 = await softUniDbContext.Employees
                           .GroupBy(e => e.JobTitle)
                           .Select(grp => new
                           {
                               JobTitle = grp.Key,
                               Salary = grp.Sum(e => e.Salary)
                           })
                           .ToListAsync();

Console.WriteLine("------------------------");
//SelectMany() example:
var addresses = await softUniDbContext.Towns
                                 .SelectMany(t => t.Addresses)
                                 .ToListAsync();

foreach (var address in addresses)
{
    Console.WriteLine($"{address.Town.TownId} {address.AddressText}");
}
