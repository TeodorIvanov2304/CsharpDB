using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoJasonDemo.Models
{   

    //Copy Json Edit->PasteSpecial-> Paste Json as Classes
    public class Rootobject
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public float Weight { get; set; }
        public Address Address { get; set; }
    }

    public class Addresss
    {
        public string City { get; set; }
        public string Street { get; set; }
    }
}


