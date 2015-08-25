using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;


namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class SCarModel
    {
        [JsonProperty(PropertyName = "C")]
        public string CarNumber { get; set; }

        [JsonProperty(PropertyName = "i")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "x")]
        public int driverID { get; set; }

        [JsonProperty(PropertyName="y")]
        public int FuelPosition { get; set; }

        [JsonProperty(PropertyName = "w")]
        public int StartValue { get; set; }

        [JsonProperty(PropertyName = "z")]
        public int StepsPerLiter { get; set; }

        [JsonProperty(PropertyName="t")]
        public string VIN { get; set; }
    }
}
