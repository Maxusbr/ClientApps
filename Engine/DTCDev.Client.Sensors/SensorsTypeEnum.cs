using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTCDev.Client.Sensors
{
    public class SensorsTypeEnum
    {
        public enum SensorsTypes
        {
            DIS_BASIC,
            DIS_DOOR,
            DIS_WINDOW,
            DIS_LEAKAGE
        }

        public enum SensorsMode
        {
            MIN,
            MAX,
            HIST
        }
    }
}
