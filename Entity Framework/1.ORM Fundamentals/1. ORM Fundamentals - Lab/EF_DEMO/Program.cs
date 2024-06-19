//Create DbContext
using EFDEMO;
using Microsoft.EntityFrameworkCore;

SoftUniDbContext context = new SoftUniDbContext();

//Create list of Employees
//In your code, using await and .ToListAsync() ensures that the database query runs asynchronously, which is crucial for maintaining responsiveness and scalability in applications that handle multiple concurrent operations.

var employees = await context.Employees
                       .Where(e => e.DepartmentId == 3)
                       .Select(e => new
                       {
                           e.FirstName,
                           e.LastName,
                           e.JobTitle,
                           DepartmentName = e.Department.Name
                       })
                       .ToListAsync();

foreach (var employee in employees)
{
    Console.WriteLine($"{employee.DepartmentName} - {employee.FirstName} {employee.LastName}: {employee.JobTitle}");
}

//Second variation, with include, but uses way more resources
//var employees = await context.Employees
//                       .Where(e => e.DepartmentId == 3)
//                       .Include(e => e.Department)       //Include info about the department (F5 Debug)
//                       .ToListAsync();

//foreach (var employee in employees)
//{
//    Console.WriteLine($"{employee.FirstName} {employee.LastName}: {employee.JobTitle}");
//}


//*************************************************************

//Make changes in DbContext
//var employee = await context.Employees.FindAsync(1);
//employee.IsDeleted = false;
//await context.SaveChangesAsync();