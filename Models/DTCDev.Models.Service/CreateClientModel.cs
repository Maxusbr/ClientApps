using DTCDev.Models.Date;
using DTCDev.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Service
{
    [JsonObject]
    public class CreateClientModel
    {
        [JsonProperty(PropertyName="A")]
        public UserLightModel User { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int IDMark { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int IDModel { get; set; }

        [JsonProperty(PropertyName = "D")]
        public int IDEngine { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int IDBody { get; set; }

        [JsonProperty(PropertyName = "F")]
        public int IDEngineType { get; set; }

        [JsonProperty(PropertyName = "G")]
        public int IDTransmission { get; set; }

        [JsonProperty(PropertyName = "H")]
        public string VIN { get; set; }

        [JsonProperty(PropertyName = "I")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "J")]
        public int Mileage { get; set; }

        [JsonProperty(PropertyName = "K")]
        public DateDataModel Date { get; set; }
    }
}
