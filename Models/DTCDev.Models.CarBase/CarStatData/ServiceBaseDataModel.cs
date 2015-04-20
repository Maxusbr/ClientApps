using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.CarStatData
{
    [JsonObject]
    public class ServiceBaseDataModel
    {
        [JsonProperty(PropertyName = "0")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "A")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int BasePeriod { get; set; }

        [JsonProperty(PropertyName = "C")]
        public DateTimeDataModel Duration { get; set; }

        [JsonProperty(PropertyName = "D")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int DistanceToMake { get; set; }

        [JsonProperty(PropertyName = "F")]
        public DateTimeDataModel CurentDate { get; set; }

        [JsonProperty(PropertyName = "G")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "H")]
        public string PeriodDimension { get; set; }

        private List<ServiceBaseDataModel> _works = new List<ServiceBaseDataModel>();
        [JsonProperty(PropertyName = "K")]
        public List<ServiceBaseDataModel> Works
        {
            get { return _works; }
            set { _works = value; }
        }
    }
}
