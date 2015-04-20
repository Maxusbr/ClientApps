using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class CarSettings
    {
        [JsonProperty(PropertyName = "A")]
        public string DID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string CarName { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string VIN { get; set; }

        [JsonProperty(PropertyName = "D")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int SendOfflineSms { get; set; }
    }
}
