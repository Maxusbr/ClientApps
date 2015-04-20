using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car
{
    public class DriverCategory
    {
        public bool A { get; set; }

        public bool B { get; set; }

        public bool C { get; set; }

        public bool D { get; set; }

        public bool E { get; set; }
    }

    public class DriverModel
    {
        public DriverModel()
        {
            Category = new DriverCategory();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int id_company { get; set; }

        public DriverCategory Category { get; set; }

        public int Lc { get; set; }

        public string SName { get; set; }

        public string FName { get; set; }
    }
}
