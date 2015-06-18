using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.User
{
    public class TestDriveUserModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string DriverId { get; set; }

        public bool Equals(TestDriveUserModel obj)
        {
            return Name == obj.Name && Email == obj.Email && Phone== obj.Phone && DriverId == obj.DriverId;
        }
    }
}
