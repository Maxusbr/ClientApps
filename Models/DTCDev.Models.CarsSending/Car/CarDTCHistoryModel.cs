using DTCDev.Models.Date;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class CarDTCHistoryModel
    {
        [JsonProperty ( PropertyName = "A")]
        public string DID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string BlockID { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string MessageCode { get; set; }

        [JsonProperty(PropertyName = "D")]
        public DateTimeDataModel Date { get; set; }

        [JsonProperty(PropertyName = "E")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "F")]
        public string Reason { get; set; }
    }
}
