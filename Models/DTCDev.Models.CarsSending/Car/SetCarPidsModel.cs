using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class SetCarPidsModel
    {
         [JsonProperty(PropertyName="A")]
         public string CarNumber { get; set; }

         private List<string> _pids = new List<string>();

         [JsonProperty(PropertyName = "B")]
         public List<string> PIDS
         {
             get { return _pids; }
             set { _pids = value; }
         }
    }
}
