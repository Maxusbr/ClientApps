using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.DeviceSender
{
    /// <summary>
    /// Класс описания подключенного сенсора
    /// </summary>
    [JsonObject]
    public class DeviceSensorsModel
    {
        [JsonProperty(PropertyName="k")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "l")]
        public int IsAnalog { get; set; }

        [JsonProperty(PropertyName = "m")]
        public int IsInput { get; set; }

        [JsonProperty(PropertyName = "n")]
        public int Min { get; set; }

        [JsonProperty(PropertyName = "o")]
        public int Max { get; set; }

        [JsonProperty(PropertyName = "p")]
        public int NormalMin { get; set; }

        [JsonProperty(PropertyName = "q")]
        public int NormalMax { get; set; }

        [JsonProperty(PropertyName = "s")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "r")]
        public int PrType { get; set; }
        
        [JsonProperty(PropertyName = "t")]
        public int Port { get; set; }
        
    }
}
