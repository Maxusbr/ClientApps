using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    public class SensorConnectionModel
    {
        public int id { get; set; }
        public int id_Device { get; set; }
        public int id_Sensor { get; set; }
        public int id_Line { get; set; }
        public int LineNumber { get; set; }

        public string Formula { get; set; }
        public string Name { get; set; }
    }
}
