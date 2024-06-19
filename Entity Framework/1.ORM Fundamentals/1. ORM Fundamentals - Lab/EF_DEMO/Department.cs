using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EFDEMO;

public class Department
{
    [Key]
    public int DepartmentId { get; set; }
    public string? Name { get; set; }
    public int ManagerId { get; set; }

    [ForeignKey(nameof(ManagerId))]
    public Employee Manager { get; set; }
    //public ICollection<Employee> Employees { get; set; }
}