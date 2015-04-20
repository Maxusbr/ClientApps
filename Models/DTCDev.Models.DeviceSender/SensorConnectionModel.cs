using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.DeviceSender
{
    /// <summary>
    /// Class for connected sensor
    /// </summary>
    public class SensorConnectionModel
    {
        public int id { get; set; }
        /// <summary>
        /// sensorID
        /// </summary>
        public int SensorID { get; set; }

        /// <summary>
        /// Port number
        /// </summary>
        public int PN { get; set; }
        
        private string _formula = "";
        /// <summary>
        /// формула рассчета величины
        /// </summary>
        public string Formula
        {
            get { return _formula; }
            set { _formula = value; }
        }

        private string _sensorName = "";

        public string SensorName
        {
            get { return _sensorName; }
            set
            {
                _sensorName = value;
            }
        }
    }
}
