using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.DeviceSender
{
    public class DeviceHistoryResponseModel
    {
        public string devID { get; set; }

        private List<DatePoint> _dp = new List<DatePoint>();
        public List<DatePoint> DP
        {
            get { return _dp; }
            set { _dp = value; }
        }
    }
}
