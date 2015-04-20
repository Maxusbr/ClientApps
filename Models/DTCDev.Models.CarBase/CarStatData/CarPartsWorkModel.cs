using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class CarPartsWorkModel
    {
        public CarPartsWorkModel()
        {
            Parts = new List<PartsTime>();
            Exemplar = new ExemplarID();
        }
        [JsonProperty(PropertyName="a")]
        public int IDWork { get; set; }
        [JsonProperty(PropertyName = "b")]
        public int Periodic { get; set; }
        [JsonProperty(PropertyName = "c")]
        public List<PartsTime> Parts { get; set; }
        [JsonProperty(PropertyName = "d")]
        public int ForAllCars { get; set; }
        [JsonProperty(PropertyName = "e")]
        public int ModelID { get; set; }
        [JsonProperty(PropertyName = "f")]
        public ExemplarID Exemplar { get; set; }


        [JsonObject]
        public class PartsTime
        {
            [JsonProperty(PropertyName = "g")]
            public int Time { get; set; }
            [JsonProperty(PropertyName = "h")]
            public int PartID { get; set; }
        }
        [JsonObject]
        public class ExemplarID
        {
            [JsonProperty(PropertyName = "n")]
            public int Saved { get; set; }
            [JsonProperty(PropertyName = "i")]
            public int Model { get; set; }
            [JsonProperty(PropertyName = "j")]
            public int Body { get; set; }
            [JsonProperty(PropertyName = "k")]
            public int Engine { get; set; }
            [JsonProperty(PropertyName = "l")]
            public int EngineType { get; set; }
            [JsonProperty(PropertyName = "m")]
            public int Transmission { get; set; }
        }
    }
}
