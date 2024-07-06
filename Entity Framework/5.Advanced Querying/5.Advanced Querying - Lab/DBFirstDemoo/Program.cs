
//Package manager command
//Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=SoftUni;Trusted_Connection=True;TrustServerCertificate = true;" Microsoft.EntityFrameworkCore.SqlServer -DataAnnotations -Context SoftUniDbContext -ContextDir Data -OutputDir Data/Models



using DBFirstDemoo.Infrastructure.Data;
using DBFirstDemoo.Infrastructure.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


using SoftUniDbContext context = new SoftUniDbContext();

//Native SQL query
int departmentId = 1;
string query = "SELECT * FROM Employees WHERE DepartmentId = " + departmentId;
var employees = await context.Employees
                                      .FromSqlRaw(query)
                                      .ToListAsync();

Console.WriteLine(employees.Count);


//SQL injection
string jobTitle = "' OR 1=1--";
string realJobTitle = "Marketing Assistant";
string queryForInj = "SELECT * FROM Employees WHERE JobTitle = '" + jobTitle + "'";

var employees2 = await context.Employees
                                      .FromSqlRaw(queryForInj)
                                      .ToListAsync();
Console.WriteLine(employees2.Count);

//Preventing the injection with placeholder --> returns 0

string injection = "' OR 1=1--";
string queryForInj2 = "SELECT * FROM Employees WHERE JobTitle = {0}";

var employees3 = await context.Employees
                                      .FromSqlRaw(queryForInj2, injection)
                                      .ToListAsync();
Console.WriteLine(employees3.Count);

//Use SQL Stored Procedure
SqlParameter percent = new SqlParameter("@percent", 10);
string queryForProcedure = "EXEC UpdateSalary @percent";
int rowsAffected = await context.Database.ExecuteSqlRawAsync(queryForProcedure, percent);

Console.WriteLine(rowsAffected);


//Detached employee from context
Employee? employee;

using (SoftUniDbContext dbContext = new SoftUniDbContext())
{
    employee = await dbContext.FindAsync<Employee>(1);
}


if (employee is not null)
{
    employee.FirstName = "Ralf";

    using SoftUniDbContext ctx = new SoftUniDbContext();

    var entry = ctx.Entry(employee);
    entry.State = EntityState.Modified;

    await ctx.SaveChangesAsync();
}

//EXPLICIT Loading
Employee employeeForExplicitLoad;
using (SoftUniDbContext dbContext = new SoftUniDbContext())
{
    employeeForExplicitLoad = await dbContext.Employees.FindAsync(1);
    var entry = dbContext.Entry(employeeForExplicitLoad);

    //Load a single address in employee
    await entry.Reference(e => e.Address).LoadAsync();

    //Load a collection of Projects in employee
    await entry.Collection(e => e.Projects).LoadAsync();
}

//EAGER Loading
using (SoftUniDbContext softContext = new SoftUniDbContext())
{
    var employeesForEagerLoad = await softContext.Employees
        .Where(d => d.DepartmentId == 1)
        .Include(d => d.Department)
        .ToListAsync();

    foreach (var e in employeesForEagerLoad)
    {
        if (e.EmployeeId == 3)
        {
            var entry = softContext.Entry(e);
            await entry.Reference(e => e.Address).LoadAsync();

            Console.WriteLine($"{e.LastName} {e.HireDate}, {e.Address.AddressText}");
        }
        else
        {
            Console.WriteLine($"{e.LastName} {e.HireDate}");
        }
    }
}

//LAZY Loading

