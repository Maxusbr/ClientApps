using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using DTCDev.Models.Date;

namespace DTCDev.Models.CarBase.CarList
{
    [JsonObject]
    public class CarListDetailsDataModel : CarListBaseDataModel
    {
        [JsonProperty(PropertyName = "K")]
        public string EngineType { get; set; }

        [JsonProperty(PropertyName = "L")]
        public string EngineVolume { get; set; }

        [JsonProperty(PropertyName = "M")]
        public string TransmissionType { get; set; }

        [JsonProperty(PropertyName = "N")]
        public Date.DateDataModel DateProduce { get; set; }

        [JsonProperty(PropertyName = "O")]
        public string CompanyID { get; set; }

        private List<WorkType> _works = new List<WorkType>();
        [JsonProperty(PropertyName = "P")]
        public List<WorkType> Works
        {
            get { return _works; }
            set { _works = value; }
        }

        [JsonProperty(PropertyName = "Q")]
        public Date.DateDataModel DatePurchase { get; set; }

        [JsonProperty(PropertyName = "R")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "S")]
        public string UserPhone { get; set; }

        [JsonObject]
        public class WorkType
        {
            [JsonProperty(PropertyName="0")]
            public string ID { get; set; }

            [JsonProperty(PropertyName = "A")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "B")]
            public int DistanceToMake { get; set; }

            [JsonProperty(PropertyName = "C")]
            public int DaysToMake { get; set; }

            [JsonProperty(PropertyName="D")]
            public int PresentModel { get; set; }

            [JsonProperty(PropertyName = "E")]
            public int PeriodicDist { get; set; }

            [JsonProperty(PropertyName = "F")]
            public int PeriodicTime { get; set; }

            [JsonProperty(PropertyName = "G")]
            public DateDataModel LastMakeTime { get; set; }

            [JsonProperty(PropertyName = "H")]
            public int LastMakeDist { get; set; }
        }
    }
}
