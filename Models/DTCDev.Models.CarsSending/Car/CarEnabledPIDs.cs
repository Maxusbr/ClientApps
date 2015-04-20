using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class CarEnabledPIDs
    {
        [JsonProperty(PropertyName="A")]
        public string PID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string Comment { get; set; }

        private int _enabled;
        [JsonProperty(PropertyName = "C")]
        public int Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (_enabled == 0)
                    PIDEnabled = false;
                else
                    PIDEnabled = true;
            }
        }

        [JsonIgnore]
        public bool PIDEnabled { get; set; }
    }
}
