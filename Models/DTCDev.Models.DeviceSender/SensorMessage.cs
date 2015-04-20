using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.DeviceSender
{
    public class SensorMessage
    {
        private Models.Date.DateTimeDataModel _dt = new Date.DateTimeDataModel();

        public Models.Date.DateTimeDataModel DT
        {
            get { return _dt; }
            set { _dt = value; }
        }

        public string DID { get; set; }

        public string Prx { get; set; }

        public int OL { get; set; }

        public int GRD { get; set; }

        private List<SensorState> _states = new List<SensorState>();

        public List<SensorState> States
        {
            get { return _states; }
            set
            {
                _states = value;
            }
        }

        private List<int> _out = new List<int>();

        public List<int> Out
        {
            get { return _out; }
            set
            {
                _out = value;
            }
        }
    }
}
