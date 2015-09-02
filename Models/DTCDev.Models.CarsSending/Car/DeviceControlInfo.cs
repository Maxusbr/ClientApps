using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class DeviceControlInfo
    {
        [JsonProperty(PropertyName = "A")]
        public string DID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int Out1 { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int Out2 { get; set; }

        [JsonProperty(PropertyName = "D")]
        public int Out3 { get; set; }
    }
}
