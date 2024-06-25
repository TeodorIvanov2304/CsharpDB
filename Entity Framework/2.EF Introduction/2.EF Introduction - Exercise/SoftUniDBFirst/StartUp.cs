using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.VisualBasic;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main()
        {
            //First we use Package manager console and type the following commands:
            /*
             Install-Package Microsoft.EntityFrameworkCore.Tools –v 6.0.1
             Install-Package Microsoft.EntityFrameworkCore.SqlServer –v 6.0.1
             Install-Package Microsoft.EntityFrameworkCore.Design -v 6.0.1
             Full command
             Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=SoftUni;Trusted_Connection=True;TrustServerCertificate = true;" Microsoft.EntityFrameworkCore.SqlServer -DataAnnotations -Context SoftUniDbContext -ContextDir Data -OutputDir Data/Models
             
             Scaffold-DbContext -Connection -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data/Models
             Finally, we want to clean up the packages we won't be using anymore from the package manager GUI or by running these commands one by one:
             Uninstall-Package Microsoft.EntityFrameworkCore.Tools -r
             Uninstall-Package Microsoft.EntityFrameworkCore.Design -RemoveDependencies
            */


            //With dependency injection we remove the new() keyword!
            using SoftUniContext context = new();

            //3.
            //Console.WriteLine(GetEmployeesFullInformation(context));

            //4.
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));

            //5
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));

            //6
            //Console.WriteLine(AddNewAddressToEmployee(context));

            //7
            //Console.WriteLine(GetEmployeesInPeriod(context));

            //8
            //Console.WriteLine(GetAddressesByTown(context));

            //9
            //Console.WriteLine(GetEmployee147(context));

            //10
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));

            //11
            //Console.WriteLine(GetLatestProjects(context));

            //12
            //Console.WriteLine(IncreaseSalaries(context));

            //13
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));

            //14
            //Console.WriteLine(DeleteProjectById(context));

            //15
            Console.WriteLine(RemoveTown(context));
        }
        public static string RemoveTown(SoftUniContext context)
        {
            const string townName = "Seattle";
            var employees = context.Employees
                                    .Where(a => a.Address.Town.Name == townName)
                                    .ToList();

            foreach (var e in employees)
            {
                e.AddressId = null; 
            }

            var addresses = context.Addresses.Where(a => a.Town.Name == townName).ToList();
            int addressesToDelete = addresses.Count;
            context.RemoveRange(addresses);
            context.Remove(context.Towns.First(t => t.Name == townName));
            context.SaveChanges();
            return $"{addressesToDelete} addresses in Seattle were deleted";
        }
        public static string DeleteProjectById(SoftUniContext context)
        {

            //1.
            //var project = context.Projects.First(p=>p.ProjectId == 2);
            //context.Remove(project);
            //context.SaveChanges();
            //var projects = context.Projects.Take(10).Select(p => p.Name).ToList();
            //StringBuilder sb = new();
            //foreach (var p in projects)
            //{
            //    sb.AppendLine(p);
            //}

            //return sb.ToString().TrimEnd();

            //2.

            var employeeProjectsToDelete = context.EmployeesProjects
                                           .Where(ep => ep.ProjectId == 2);
            context.RemoveRange(employeeProjectsToDelete);

            context.Projects.Remove(context.Projects.Find(2));

            context.SaveChanges();

            var projects = context.Projects
                            .Take(10)
                            .Select(p => p.Name)
                            .ToArray();

            return string.Join(Environment.NewLine, projects);

        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employeesWithSa = context.Employees
                                  .OrderBy(f => f.FirstName)
                                  .ThenBy(l => l.LastName)
                                  .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                                  .Select(e => new
                                  {
                                      e.FirstName,
                                      e.LastName,
                                      e.JobTitle,
                                      e.Salary
                                  })
                                  .ToList();
            StringBuilder sb = new();
            foreach (var e in employeesWithSa)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employeesToRaise = context.Employees
                                   .Where(d => d.Department.Name == "Engineering" ||
                                             d.Department.Name == "Tool Design" ||
                                             d.Department.Name == "Marketing" ||
                                             d.Department.Name == "Information Services")
                                   .Select(e => new
                                   {
                                       e.FirstName,
                                       e.LastName,
                                       IncreasedSalary = e.Salary * 1.12m
                                   })
                                   .OrderBy(e => e.FirstName)
                                   .ThenBy(e => e.LastName)
                                   .ToList();
            context.SaveChanges();
            StringBuilder sb = new();
            foreach (var e in employeesToRaise)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.IncreasedSalary:F2})");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            var latestProjects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var project in latestProjects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine($"{project.StartDate:M/d/yyyy h:mm:ss tt} ");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            List<Department> departments = context.Departments
                              .Include(x => x.Employees)
                              .Where(e => e.Employees.Count > 5)
                              .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var department in departments.OrderBy(x => x.Employees.Count).ThenBy(x => x.Name))
            {
                sb.AppendLine($"{department.Name} - {department.Manager.FirstName} {department.Manager.LastName}");

                foreach (var employee in department.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            //Include == JOIN
            var employee = context.Employees
           .Include(e => e.EmployeesProjects)
           .ThenInclude(ep => ep.Project)
           .FirstOrDefault(e => e.EmployeeId == 147);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var employeeProject in employee.EmployeesProjects
                         .OrderBy(ep => ep.Project.Name))
            {
                sb.AppendLine(employeeProject.Project.Name);
            }


            return sb.ToString().TrimEnd();
        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                       .OrderByDescending(a => a.Employees.Count)
                       .ThenBy(a => a.Town.Name)
                       .ThenBy(a => a.AddressText)
                       .Take(10)
                       .Select(a => new
                       {
                           a.AddressText,
                           EmployeesCount = a.Employees.Count,
                           TownName = a.Town.Name
                       })
                       .ToList();

            StringBuilder sb = new();
            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employeesInPeriod = context.Employees
                        .Select(e => new
                        {
                            //We can give a name to a variable, like allias(AS) in SQL
                            EmployeeNames = $"{e.FirstName} {e.LastName}",
                            ManagerNames = $"{e.Manager.FirstName} {e.Manager.LastName}",
                            //We have to do the year comparison in the Select projection!
                            Projets = e.EmployeesProjects
                            .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                            .Select(ep => new //Double projection
                            {
                                ProjectName = ep.Project.Name,
                                ep.Project.StartDate,
                                ep.Project.EndDate
                            })
                        })
                        .Take(10)
                        .ToList();

            StringBuilder sb = new();

            foreach (var e in employeesInPeriod)
            {
                //Using the newly named variables
                sb.AppendLine($"{e.EmployeeNames} - Manager: {e.ManagerNames}");

                if (e.Projets.Any())
                {
                    foreach (var p in e.Projets)
                    {
                        if (p.EndDate == null)
                        {
                            //Formatting the StartDate with : just like :F2
                            sb.AppendLine($"--{p.ProjectName} - {p.StartDate:M/d/yyyy h:mm:ss tt} - not finished");
                        }
                        else
                        {
                            sb.AppendLine($"--{p.ProjectName} - {p.StartDate:M/d/yyyy h:mm:ss tt} - {p.EndDate:M/d/yyyy h:mm:ss tt}");
                        }
                    }
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            //Create new Address
            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            //Find Nakov employee
            Employee employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            if (employee is not null)
            {
                employee.Address = newAddress;
                context.SaveChanges();
            }

            List<string> employees = context.Employees
                                   .OrderByDescending(e => e.AddressId)
                                   .Take(10)
                                   .Select(e => e.Address.AddressText)
                                   .ToList();

            return string.Join(Environment.NewLine, employees);
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employeesFromResearchAndDevelopment = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary,
                    e.Department
                })
                .Where(d => d.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employeesFromResearchAndDevelopment)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:F2}");
            }
            return sb.ToString().TrimEnd();

        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            //Option 1
            //return string.Join(Environment.NewLine, context.Employees
            //             .OrderBy(e => e.EmployeeId)
            //             .Select(e => $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}")
            //             .ToList());

            //Option 2
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList();

            StringBuilder sb = new();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }
            return sb.ToString().Trim();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(s => s.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}