using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Order
{
    [JsonObject]
    public class WorkChangeModel
    {
        [JsonProperty(PropertyName = "A")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int NewCost { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string Coment { get; set; }
    }
}
