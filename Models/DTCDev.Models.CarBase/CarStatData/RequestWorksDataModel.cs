using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class RequestWorksDataModel
    {
        [JsonProperty(PropertyName="A")]
        public int idModel { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int idEngine { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int idEngineType { get; set; }

        [JsonProperty(PropertyName = "D")]
        public int idBody { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int idTransmission { get; set; }
    }
}
