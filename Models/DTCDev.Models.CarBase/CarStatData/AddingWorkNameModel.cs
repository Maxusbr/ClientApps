using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class AddingWorkNameModel
    {
        [JsonProperty(PropertyName = "A")]
        public int idType { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string Name { get; set; }

        [JsonProperty(PropertyName="C")]
        public int isPeriodic { get; set; }
    }
}
