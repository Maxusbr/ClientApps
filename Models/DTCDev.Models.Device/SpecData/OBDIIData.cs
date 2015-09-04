using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device.SpecData
{
    public class OBDIIData
    {
        public bool OBDConnect { get; set; }

        public float Power { get; set; }

        public int EngineRpm { get; set; }

        public int Speed { get; set; }

        public int Temp { get; set; }

        public int DTCDistance { get; set; }

        public int MilStatus { get; set; }

        public int MilDistance { get; set; }

        public int DTCsCount { get; set; }

        public string DTCRow { get; set; }

        public int FuelLevel { get; set; }

        public int FuelCalc { get; set; }
    }
}
