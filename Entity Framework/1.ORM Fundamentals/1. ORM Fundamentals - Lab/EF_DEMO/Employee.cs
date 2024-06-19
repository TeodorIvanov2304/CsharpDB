using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDEMO
{
    public class Employee
    {
        //Primary key
        [Key]
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        //We write ? after the property type, because the string can be null
        public string? MiddleName { get; set; }
        public string? JobTitle { get; set; }

        public int DepartmentId { get; set; }

        public int ManagerId { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }

        public int AddressId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }

        [ForeignKey(nameof(ManagerId))]
        public Employee Manager { get; set; }
    }
}
