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
            PostTypes = new List<DicDataModel>();
        }

        [JsonProperty(PropertyName = "a")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "b")]
        public List<DepModel> Departments { get; set; }

        public List<DicDataModel> PostTypes { get; set; }


        [JsonObject]
        public class DepModel
        {
            public DepModel()
            {
                Posts = new List<PostSettings>();
            }

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

            public List<PostSettings> Posts { get; set; }

        }

        [JsonObject]
        public class PostSettings
        {
            [JsonProperty(PropertyName = "k")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "l")]
            public int TimeFrom { get; set; }

            [JsonProperty(PropertyName = "m")]
            public int TimeTo { get; set; }

            [JsonProperty(PropertyName = "n")]
            public int ID { get; set; }


            [JsonProperty(PropertyName = "o")]
            public int idPostType { get; set; }

            [JsonProperty(PropertyName = "p")]
            public int IDDep { get; set; }
        }
    }
}
