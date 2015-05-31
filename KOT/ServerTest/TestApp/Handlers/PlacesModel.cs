using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KOTServerTester.Handlers
{
    [JsonObject]
    public class PlacesModel
    {
        [JsonProperty(PropertyName = "A")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string Adress { get; set; }

        [JsonProperty(PropertyName = "D")]
        public int Latitude { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int Longitude { get; set; }

        [JsonProperty(PropertyName = "F")]
        public int MinPrice { get; set; }

        [JsonProperty(PropertyName = "G")]
        public int Score { get; set; }

        [JsonProperty(PropertyName = "H")]
        public int Comments { get; set; }

        [JsonProperty(PropertyName = "I")]
        public int idCategory { get; set; }
    }
}
