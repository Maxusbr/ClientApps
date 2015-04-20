using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class CarStatInfoModel
    {
        [JsonProperty(PropertyName = "a")]
        public string InterfaceType { get; set; }

        private List<PIDInfo> _pids = new List<PIDInfo>();
        [JsonProperty(PropertyName = "b")]
        public List<PIDInfo> PIDs
        {
            get { return _pids; }
            set { _pids = value; }
        }

        [JsonProperty(PropertyName = "e")]
        public string ProduceYears { get; set; }

        [JsonObject]
        public class PIDInfo
        {
            [JsonProperty(PropertyName = "c")]
            public string PID { get; set; }

            [JsonProperty(PropertyName = "d")]
            public string Comment { get; set; }
        }
    }
}
