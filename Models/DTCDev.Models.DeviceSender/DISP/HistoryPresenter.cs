using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.DeviceSender.DISP
{
    public class HistoryPresenter
    {
        public int id { get; set; }

        public string DID { get; set; }

        public string Name { get; set; }

        private List<DeviceSensorsModel> _sensors = new List<DeviceSensorsModel>();

        public List<DeviceSensorsModel> Sensors
        {
            get{return _sensors;}
            set{_sensors = value;}
        }

        private List<DatePoint> _data = new List<DatePoint>();

        public List<DatePoint> Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
