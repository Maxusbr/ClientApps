using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Report
{
    [JsonObject]
    public class WorkTimeReportModel
    {
        private DateDataModel _date = new DateDataModel();
        [JsonProperty(PropertyName="a")]
        public DateDataModel Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private TimeModel _startWork = new TimeModel();
        private TimeModel _stopWork = new TimeModel();

        [JsonProperty(PropertyName = "b")]
        public TimeModel StartWork
        {
            get { return _startWork; }
            set { _startWork = value; }
        }

        [JsonProperty(PropertyName = "c")]
        public TimeModel StopWork
        {
            get { return _stopWork; }
            set { _stopWork = value; }
        }

        [JsonProperty(PropertyName = "d")]
        public int UsedInworkTime { get; set; }

        [JsonProperty(PropertyName = "e")]
        public int UsedInUnworkTime { get; set; }

        [JsonProperty(PropertyName = "f")]
        public int DistWorkTime { get; set; }

        [JsonProperty(PropertyName = "j")]
        public int DistUnworkTime { get; set; }
    }
}
