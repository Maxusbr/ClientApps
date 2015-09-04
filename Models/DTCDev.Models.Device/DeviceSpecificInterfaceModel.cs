using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{

    public class DeviceSpecificInterfaceModel
    {
        public int id { get; set; }
        public bool PRX { get; set; }
        public bool GPS { get; set; }
        public bool ACS { get; set; }
        public bool MRC { get; set; }
        public bool WTR { get; set; }

        public int Proxy { get; set; }
        public int ProxyDate { get; set; }
        public int ElectricCounter { get; set; }
        public int WaterCounter { get; set; }
        public int AccelerometerX { get; set; }
        public int AccelerometerY { get; set; }
        public int AccelerometerZ { get; set; }
        public int GPSX { get; set; }
        public int GPSY { get; set; }
        public int GPSSpeed { get; set; }
        public int GPSAsimut { get; set; }
    }
}
