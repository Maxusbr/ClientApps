using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Adress
{
    public class AdressSaveModel
    {
        public int idExist_Country { get; set; }
        public string Country { get; set; }

        public int idExist_Region { get; set; }
        public string Region { get; set; }

        public int idExist_State { get; set; }
        public string State { get; set; }

        public int idExist_City { get; set; }
        public string City { get; set; }

        public int idExist_Street { get; set; }
        public string Street { get; set; }

        public string Place { get; set; }

        public int Latitude { get; set; }

        public int Langitude { get; set; }
    }
}
