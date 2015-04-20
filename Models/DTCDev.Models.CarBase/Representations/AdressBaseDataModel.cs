using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.Representations
{
    [JsonObject]
    public class AdressBaseDataModel
    {
        [JsonProperty(PropertyName = "A")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string District { get; set; }

        [JsonProperty(PropertyName = "D")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "E")]
        public string Street { get; set; }

        [JsonProperty(PropertyName = "F")]
        public string House { get; set; }

    }
}
