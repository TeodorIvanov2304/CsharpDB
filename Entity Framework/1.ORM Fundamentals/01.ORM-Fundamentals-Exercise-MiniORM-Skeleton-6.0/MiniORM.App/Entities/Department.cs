namespace MiniORM.App.Entities;

using System.ComponentModel.DataAnnotations;

public class Department
{
    [Key] public int ID { get; set; }
    [Required] public string Name { get; set; }

    public ICollection<Employee> Employees { get; set; }
}