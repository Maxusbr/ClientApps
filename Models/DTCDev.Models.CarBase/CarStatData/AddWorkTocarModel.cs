using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class AddWorkTocarModel
    {
        [JsonProperty(PropertyName = "A")]
        public int Model { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int Transmission { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int Body { get; set; }

        [JsonProperty(PropertyName = "D")]
        public int Engine { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int EngineType { get; set; }

        [JsonProperty(PropertyName = "F")]
        public string WorkName { get; set; }

        [JsonProperty(PropertyName = "G")]
        public int Periodic { get; set; }

        [JsonProperty(PropertyName = "H")]
        public int Distance { get; set; }
    }
}
