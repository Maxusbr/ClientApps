using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class CurrentDevicesDataModel
    {
        [JsonProperty(PropertyName = "A")]
        public string DevID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public TrackerTypes.TTypes TType { get; set; }
    }
}
