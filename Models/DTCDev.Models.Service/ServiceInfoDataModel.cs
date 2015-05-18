using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Service
{
    [JsonObject]
    public class ServiceInfoDataModel
    {
        public ServiceInfoDataModel()
        {
            Departments = new List<DepModel>();
        }

        [JsonProperty(PropertyName = "a")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "b")]
        public List<DepModel> Departments { get; set; }


        [JsonObject]
        public class DepModel
        {

            [JsonProperty(PropertyName = "i")]
            public int id { get; set; }

            [JsonProperty(PropertyName = "c")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "d")]
            public string Phone { get; set; }

            [JsonProperty(PropertyName = "e")]
            public int From { get; set; }

            [JsonProperty(PropertyName = "f")]
            public int To { get; set; }

            [JsonProperty(PropertyName = "g")]
            public int NHCost { get; set; }

            [JsonProperty(PropertyName = "h")]
            public string Adress { get; set; }
        }
    }
}
