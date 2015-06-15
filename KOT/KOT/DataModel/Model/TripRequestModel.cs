using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class TripRequestModel
    {
        [JsonProperty(PropertyName = "A")]
        public DateTimeDataModel Start { get; set; }

        [JsonProperty(PropertyName = "B")]
        public DateTimeDataModel Stop { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string devID { get; set; }
    }

}
