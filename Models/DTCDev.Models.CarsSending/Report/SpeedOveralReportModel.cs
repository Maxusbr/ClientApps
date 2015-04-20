using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Report
{
    [JsonObject]
    public class SpeedOveralReportModel
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
            public class SpeedPoint : INotifyPropertyChanged
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

                [JsonProperty(PropertyName = "f")]
                public int TimeMove { get; set; }

                [JsonProperty(PropertyName = "g")]
                public int DistanceMove { get; set; }

                [JsonProperty(PropertyName = "h")]
                public int LatitudeStart { get; set; }

                [JsonProperty(PropertyName = "i")]
                public int LongitudeStart { get; set; }

                [JsonProperty(PropertyName = "j")]
                public int LatitudeStop { get; set; }

                [JsonProperty(PropertyName = "k")]
                public int LongitudeStop { get; set; }

                private string _startAdress = string.Empty;
                [JsonIgnore]
                public string StartAdress { get { return _startAdress; }
                    set
                    {
                        if (_startAdress.Equals(value)) return;
                        _startAdress = value;
                        OnPropertyChanged("StartAdress");
                    } }

                [JsonIgnore]
                public string StopAdress { get; set; }


                public event PropertyChangedEventHandler PropertyChanged;
                private void OnPropertyChanged(string propertyName)
                {
                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        var e = new PropertyChangedEventArgs(propertyName);
                        handler(this, e);
                    }
                }
            }
        }
    }
}
