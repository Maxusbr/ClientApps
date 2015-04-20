using DTCDev.Models.Date;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Report
{
    [JsonObject]
    public class ReportFuelModel
    {
        [JsonProperty(PropertyName="A")]
        public string DID { get; set; }

        private List<ItemModel> _report = new List<ItemModel>();
        [JsonProperty(PropertyName="B")]
        public List<ItemModel> Report
        {
            get { return _report; }
            set { _report = value; }
        }

        [JsonObject]
        public class ItemModel
        {
            DateTimeDataModel _dt = new DateTimeDataModel();
            [JsonProperty(PropertyName="L")]
            public DateTimeDataModel DT
            {
                get { return _dt; }
                set { _dt = value; }
            }

            [JsonProperty(PropertyName="V")]
            public int Vol { get; set; }
        }
    }
}
