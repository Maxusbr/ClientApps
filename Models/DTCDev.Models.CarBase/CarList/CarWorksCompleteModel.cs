using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.CarList
{
    [JsonObject]
    public class CarWorksCompleteModel
    {
        [JsonProperty(PropertyName="A")]
        public string CarNumber { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "D")]
        public DateDataModel Date { get; set; }

        [JsonProperty(PropertyName = "I")]
        public int OrderNo { get; set; }

        [JsonProperty(PropertyName = "J")]
        public int DistanceMake { get; set; }

        private List<WorkItemModel> _workIds = new List<WorkItemModel>();
        [JsonProperty(PropertyName = "E")]
        public List<WorkItemModel> WorkIds
        {
            get { return _workIds; }
            set { _workIds = value; }
        }

        [JsonObject]
        public class WorkItemModel
        {
            [JsonProperty(PropertyName = "F")]
            public int Cost { get; set; }

            [JsonProperty(PropertyName = "G")]
            public string Comment { get; set; }

            [JsonProperty(PropertyName = "H")]
            public string id { get; set; }
        }
    }
}
