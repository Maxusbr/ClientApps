using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Navigation
{
    [JsonObject]
    public class SNaviData
    {
        [JsonProperty(PropertyName = "A")]
        public int Latitude { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int Longitude { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int Sattelites { get; set; }

        [JsonProperty(PropertyName = "D")]
        public int Speed { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int Direction { get; set; }
    }
}
