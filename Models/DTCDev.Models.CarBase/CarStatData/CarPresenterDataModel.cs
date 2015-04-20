using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class CarPresenterDataModel
    {
        [JsonProperty(PropertyName = "A")]
        public int carID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string CarNumber { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string Mark { get; set; }

        [JsonProperty(PropertyName = "D")]
        public string Model { get; set; }

        [JsonProperty(PropertyName = "E")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "F")]
        public string Engine { get; set; }

        [JsonProperty(PropertyName = "G")]
        public string ControllerID { get; set; }

        [JsonProperty(PropertyName = "X")]
        public int id_Mark { get; set; }


        [JsonProperty(PropertyName = "S")]
        public int id_Model { get; set; }

        [JsonProperty(PropertyName = "T")]
        public int id_Body { get; set; }

        [JsonProperty(PropertyName = "U")]
        public int id_Engine { get; set; }

        [JsonProperty(PropertyName = "W")]
        public int id_Transmission { get; set; }

        [JsonProperty(PropertyName = "V")]
        public int id_EngineType { get; set; }
    }
}