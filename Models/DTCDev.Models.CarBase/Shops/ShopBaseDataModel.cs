using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.Shops
{
    [JsonObject]
    public class ShopBaseDataModel
    {
        [JsonProperty(PropertyName = "A")]
        public DateDataModel Dates { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string TypeAuto { get; set; }

        [JsonProperty(PropertyName = "E")]
        public string Comments { get; set; }
    }
}
