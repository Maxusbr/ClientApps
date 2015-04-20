using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DTCDev.Models.DeviceSender
{
    public class DatePoint
    {
        private DTCDev.Models.Date.DateTimeDataModel _date = new Date.DateTimeDataModel();

        public DTCDev.Models.Date.DateTimeDataModel Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private List<SensorState> _states = new List<SensorState>();

        public List<SensorState> States
        {
            get { return _states; }
            set { _states = value; }
        }

        private string _proxy = "";

        public string Prx
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        private int _grd = 0;

        public int GRD
        {
            get { return _grd; }
            set { _grd = value; }
        }
    }
}
