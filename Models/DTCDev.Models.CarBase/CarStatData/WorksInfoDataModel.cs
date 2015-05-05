using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class WorksInfoDataModel
    {
        [JsonProperty(PropertyName = "A")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int id_Class { get; set; }

        [JsonProperty(PropertyName = "I")]
        public int NH { get; set; }

        [JsonIgnore]
        public decimal NHD { get; set; }

        [JsonProperty(PropertyName = "S")]
        public int Cost { get; set; }

        [JsonProperty(PropertyName = "H")]
        public string WGUID { get; set; }

        [JsonProperty(PropertyName="X")]
        public int idWork { get; set; }
    }

    [JsonObject]
    public class WorksInfoExemplarDataModel : WorksInfoDataModel
    {
        [JsonProperty(PropertyName = "D")]
        public int idRow { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int PeriodicDistance { get; set; }

        [JsonProperty(PropertyName = "F")]
        public int PeriodicMonth { get; set; }
    }

    [JsonObject]
    public class WorksWithFlagDataModel : WorksInfoDataModel
    {
        [JsonProperty(PropertyName = "G")]
        public int IsPeriodic { get; set; }
    }
}
