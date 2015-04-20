using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.Users
{
    public  class UsersModel
    {
        public int id;
        public int id_Company;
        public int id_Role;
        public string UID;
        
        public string RoleName { get; set; }
        public string Name { get; set; }
        public List<UsersRoleModel> Role { get; set; }
    }
}
