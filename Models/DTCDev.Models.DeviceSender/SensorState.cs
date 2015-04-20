using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.DeviceSender
{
    [JsonObject]
    public class SensorState
    {
        [JsonProperty(PropertyName="1")]
        public int id { get; set; }
        [JsonProperty(PropertyName = "2")]
        public int Vol { get; set; }
    }
}
