namespace MiniORM.App.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee
{
    [Key] public int ID { get; set; }
    [Required] public string FirstName { get; set; }
    public string MiddleName { get; set; }
    [Required] public string LastName { get; set; }
    public bool IsEmployed { get; set; }

    [ForeignKey(nameof(Department))] public int DepartmentID { get; set; }
    public Department Department { get; set; }

    public ICollection<EmployeeProject> EmployeeProjects { get; set; }
}