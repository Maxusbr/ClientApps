using DTCDev.Models.Date;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class CarStateOBDModel
    {
        [JsonProperty(PropertyName="A")]
        public string DID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string VIN { get; set; }

        private List<KVP> _obd = new List<KVP>();
        [JsonProperty(PropertyName = "C")]
        public List<KVP> OBD
        {
            get { return _obd; }
            set { _obd = value; }
        }

        private List<KVP> _errors = new List<KVP>();
        [JsonProperty(PropertyName = "D")]
        public List<KVP> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        private DateTimeDataModel _dateUpdate = new DateTimeDataModel();

        [JsonProperty(PropertyName = "G")]
        public DateTimeDataModel DateUpdate
        {
            get { return _dateUpdate; }
            set { _dateUpdate = value; }
        }


        [JsonObject]
        public class KVP
        {
            [JsonProperty(PropertyName = "E")]
            public string Key { get; set; }

            [JsonProperty(PropertyName = "F")]
            public string Value { get; set; }
        }
    }
}
