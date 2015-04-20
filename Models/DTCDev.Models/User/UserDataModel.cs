using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.User
{
    public class UserDataModel
    {
        public string UID { get; set; }

        public string TempUID { get; set; }

        public int id { get; set; }

        public int id_Company { get; set; }

        public string ComanyGUID { get; set; }

        public string CompanyName { get; set; }

        public int AuthCounter { get; set; }
    }
}
