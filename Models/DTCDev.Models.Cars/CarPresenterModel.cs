using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car
{
    public class CarPresenterModel:AbstractModels.AbstractCar
    {
        public string Model { get; set; }
        public string Mark { get; set; }
        public int EngineVolume { get; set; }
        public string EngineType { get; set; }
        public string Body { get; set; }
        public string TransmissionType { get; set; }
    }
}
