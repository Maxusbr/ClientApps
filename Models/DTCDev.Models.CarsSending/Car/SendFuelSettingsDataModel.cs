using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class SendFuelSettingsDataModel
    {
        [JsonProperty(PropertyName = "A")]
        public int Position { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int StartValue { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int Scale { get; set; }

        [JsonProperty(PropertyName = "D")]
        public string CarID { get; set; }
    }
}
