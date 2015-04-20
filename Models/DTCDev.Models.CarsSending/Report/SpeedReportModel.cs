using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Report
{
    [JsonObject]
    public class SpeedReportModel
    {
        private List<DayReport> _data = new List<DayReport>();
        [JsonProperty(PropertyName = "a")]
        public List<DayReport> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        [JsonObject]
        public class DayReport
        {
            private DateDataModel _date = new DateDataModel();

            [JsonProperty(PropertyName = "b")]
            public DateDataModel Date
            {
                get { return _date; }
                set { _date = value; }
            }

            private List<SpeedPoint> _points = new List<SpeedPoint>();

            [JsonProperty(PropertyName = "c")]
            public List<SpeedPoint> Points
            {
                get { return _points; }
                set { _points = value; }
            }

            [JsonObject]
            public class SpeedPoint
            {
                private TimeModel _time = new TimeModel();

                [JsonProperty(PropertyName = "d")]
                public TimeModel Time
                {
                    get { return _time; }
                    set { _time = value; }
                }


                [JsonProperty(PropertyName = "e")]
                public int Speed { get; set; }
            }
        }
    }
}
