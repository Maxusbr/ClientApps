using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Report
{
    [JsonObject]
    public class ParkingReportModel
    {
        [JsonProperty(PropertyName="a")]
        public DateDataModel Date { get; set; }

        [JsonProperty(PropertyName = "b")]
        public int Latitude { get; set; }

        [JsonProperty(PropertyName = "c")]
        public int Longitude { get; set; }

        [JsonProperty(PropertyName = "d")]
        public TimeModel Start { get; set; }

        [JsonProperty(PropertyName = "e")]
        public TimeModel Stop { get; set; }
    }
}
