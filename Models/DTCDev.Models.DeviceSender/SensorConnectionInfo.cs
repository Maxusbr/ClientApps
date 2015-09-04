using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.DeviceSender
{
    [JsonObject]
    public class SensorConnectionInfo
    {
        [JsonProperty(PropertyName = "A")]
        public string DID { get; set; }

        private List<ConnectionInfo> _connections = new List<ConnectionInfo>();

        [JsonProperty(PropertyName = "B")]
        public List<ConnectionInfo> Connections
        {
            get { return _connections; }
            set { _connections = value; }
        }

        [JsonObject]
        public class ConnectionInfo
        {
            [JsonProperty(PropertyName = "C")]
            public int Port { get; set; }

            [JsonProperty(PropertyName = "D")]
            public int idSensor { get; set; }
        }
    }
}
