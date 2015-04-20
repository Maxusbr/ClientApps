using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending
{
    [JsonObject]
    public class DriverModel
    {
        [JsonProperty(PropertyName="1")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "2")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "3")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "4")]
        public int id_company { get; set; }

        [JsonProperty(PropertyName = "5")]
        public int CatA { get; set; }
        [JsonProperty(PropertyName = "6")]
        public int CatB { get; set; }
        [JsonProperty(PropertyName = "7")]
        public int CatC { get; set; }
        [JsonProperty(PropertyName = "8")]
        public int CatD { get; set; }
        [JsonProperty(PropertyName = "9")]
        public int CatE { get; set; }

        [JsonProperty(PropertyName = "0")]
        public int Lc { get; set; }

        [JsonProperty(PropertyName = "a")]
        public string SName { get; set; }

        [JsonProperty(PropertyName = "b")]
        public string FName { get; set; }
    }
}
