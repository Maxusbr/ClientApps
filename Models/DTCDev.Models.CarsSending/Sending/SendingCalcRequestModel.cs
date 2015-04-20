using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Sending
{
    [JsonObject]
    public class SendingCalcRequestModel
    {
        [JsonProperty(PropertyName="A")]
        public int Mode { get; set; }
         
        [JsonProperty(PropertyName="B")]
        public int Param { get; set; }
    }
}
