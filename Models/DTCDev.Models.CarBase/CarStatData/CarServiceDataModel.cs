using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class CarServiceDataModel : CarPresenterDataModel
    {
        [JsonProperty(PropertyName = "1")]
        public string EngineType { get; set; }

        [JsonProperty(PropertyName = "2")]
        public int EngineVolume { get; set; }

        [JsonProperty(PropertyName = "3")]
        public string TransmissionType { get; set; }

        [JsonProperty(PropertyName = "4")]
        public int CurrentDistance { get; set; }

        [JsonProperty(PropertyName = "5")]
        public Date.DateDataModel DateProduce { get; set; }

        [JsonProperty(PropertyName = "6")]
        public Date.DateDataModel DatePurchase { get; set; }

        [JsonProperty (PropertyName = "7")]
        public string VIN { get; set; }

        [JsonProperty(PropertyName = "8")]
        public string ClientName { get; set; }

        [JsonProperty(PropertyName = "9")]
        public string ClientPhone { get; set; }

        [JsonProperty(PropertyName = "10")]
        public string ClientMail { get; set; }

        [JsonProperty(PropertyName = "11")]
        public string ClientLogin { get; set; }

        [JsonProperty(PropertyName = "12")]
        public string DID { get; set; }
    }
}
