using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device.SpecData
{
    public class SpeedDataModel
    {
        public SpeedDataModel()
        {
            Discr = new List<bool>();
            Anlg = new List<int>();
        }

        public DateTime dt { get; set; }

        public string deviceID { get; set; }


        /// <summary>
        /// Speed
        /// </summary>
        public int Sp { get; set; }

        /// <summary>
        /// GPSx
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// GPSy
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Azimut
        /// </summary>
        public int Azm { get; set; }

        /// <summary>
        /// Acs x
        /// </summary>
        public int AX { get; set; }

        /// <summary>
        /// Acs y
        /// </summary>
        public int AY { get; set; }


        /// <summary>
        /// Acs z
        /// </summary>
        public int AZ { get; set; }


        /// <summary>
        /// Satellite count
        /// </summary>
        public int Sat { get; set; }

        public int Alt { get; set; }

        public float Lat { get; set; }

        public float Lon { get; set; }

        public int Distance { get; set; }

        public int OL { get; set; }

        public List<int> Anlg { get; set; }

        public List<bool> Discr { get; set; }
    }
}
