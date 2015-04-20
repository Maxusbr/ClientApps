using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Service
{
    [JsonObject]
    public class MasterDataModel
    {
        [JsonProperty(PropertyName="A")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int ID { get; set; }
    }
}
