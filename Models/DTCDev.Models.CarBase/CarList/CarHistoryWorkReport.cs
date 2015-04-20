using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.CarList
{
    [JsonObject]
    public class CarHistoryWorkReport
    {
        [JsonProperty(PropertyName="B")]
        public int Distance { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "D")]
        public DateDataModel Date { get; set; }

        [JsonProperty(PropertyName = "F")]
        public int Cost { get; set; }

        [JsonProperty(PropertyName = "G")]
        public string WorkName { get; set; }

        [JsonProperty(PropertyName = "H")]
        public int OrderN { get; set; }
    }
}
