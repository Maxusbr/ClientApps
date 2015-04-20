using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.Cars
{
    public class CarExemplarModel
    {
        public int id { get; set; }
        public int id_Model { get; set; }
        public int id_Transmission { get; set; }
        public int id_Engine { get; set; }
        public int id_Body { get; set; } // Пока не пофиксят баг с БД
        public int id_EngineType { get; set; }

        public string Transmission { get; set; }

        public string Engine { get; set; }

        public string Body { get; set; }

        public string EngineType { get; set; }

        public string Model { get; set; }

        public string Mark { get; set; }

        public string CarGuid { get; set; }
    }
}
