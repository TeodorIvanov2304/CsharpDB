using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoJasonDemo.Models
{
    public class Person
    {
        public string FullName { get; set; }
        public uint Age { get; set; }
        public  uint Height { get; set; }
        public decimal Weight { get; set; }
        public Address Address { get; set; }
    }
}
