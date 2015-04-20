using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.DeviceSender
{
    [JsonObject]
    public class DeviceDataModel
    {
        [JsonProperty(PropertyName = "a")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "b")]
        public string DID { get; set; }

        [JsonProperty(PropertyName = "c")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "d")]
        public int CN { get; set; }

        [JsonProperty(PropertyName = "e")]
        public List<DeviceSensorsModel> Sensors { get; set; }
    }
}
