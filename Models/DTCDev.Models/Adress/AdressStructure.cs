using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Adress
{
    public class AdressStructure
    {
        public int id { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string State { get; set; }

        public string SmallRegion { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public double Latitude { get; set; }

        public double Langitude { get; set; }
    }
}
