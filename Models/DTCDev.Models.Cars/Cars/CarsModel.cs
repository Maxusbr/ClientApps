using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.Cars
{
    public class CarsModel:AbstractModels.AbstractCar
    {
        public int id { get; set; }
        
        public string Number { get; set; }
        public int id_Device { get; set; }
        public int id_CarExemplar { get; set; }
        public int id_Company { get; set; }
    }
}
