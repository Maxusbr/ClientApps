using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.CarList
{
    [JsonObject]
    public class CarListBaseDataModel
    {
        [JsonProperty(PropertyName = "0")]
        public string DID { get; set; }

        [JsonProperty(PropertyName = "A")]
        public string CarNumber { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string Mark { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string Model { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int CurrentDistance { get; set; }

        [JsonProperty(PropertyName = "F")]
        public int IncomeDistance { get; set; }

        [JsonProperty(PropertyName = "G")]
        public string ServiceName { get; set; }

        [JsonProperty(PropertyName = "H")]
        public int DaysToService { get; set; }

        [JsonProperty(PropertyName = "I")]
        public int DTCErrCount { get; set; }

        [JsonProperty(PropertyName = "j")]
        public int ID { get; set; }

        public override string ToString()
        {
            return string.Format("{1} {2} ({0})", CarNumber, Mark, Model);
        }
    }
}
