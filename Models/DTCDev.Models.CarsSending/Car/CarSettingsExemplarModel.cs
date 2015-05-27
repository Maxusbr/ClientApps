using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class CarSettingsExemplarModel
    {
        [JsonProperty(PropertyName="A")]
        public int IDMark { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int IDModel { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int IDEngine { get; set; }

        [JsonProperty(PropertyName = "D")]
        public int IDBody { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int IDEngineType { get; set; }

        [JsonProperty(PropertyName = "F")]
        public int IDTransmission { get; set; }

        [JsonProperty(PropertyName = "G")]
        public string Years { get; set; }

        [JsonProperty(PropertyName = "H")]
        public string ProtocolType { get; set; }
    }
}
