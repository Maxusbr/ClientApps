using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class CarStateFullModel
    {
        private SCarData _state = new SCarData();
        [JsonProperty (PropertyName = "A")]
        public SCarData State
        {
            get { return _state; }
            set { _state = value; }
        }

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
