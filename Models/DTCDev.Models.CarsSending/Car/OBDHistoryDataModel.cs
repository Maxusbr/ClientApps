using DTCDev.Models.Date;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class OBDHistoryDataModel
    {
        [JsonProperty(PropertyName = "a")]
        public string DevID { get; set; }

        [JsonProperty(PropertyName = "b")]
        public DateDataModel DT { get; set; }

        private List<OBDParam> _data = new List<OBDParam>();

        [JsonProperty(PropertyName = "f")]
        public List<OBDParam> Data
        {
            get { return _data; }
            set
            {
                _data = value;
            }
        }


        [JsonObject]
        public class OBDParam
        {
            [JsonProperty(PropertyName = "e")]
            public TimeModel Time { get; set; }

            [JsonProperty(PropertyName = "c")]
            public string Code { get; set; }

            [JsonProperty(PropertyName = "d")]
            public int Vol { get; set; }
        }
    }
}
