using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class TripAdvisorModel
    {
        [JsonProperty(PropertyName = "X")]
        public string DID { get; set; }

        [JsonProperty(PropertyName = "A")]
        public int CurrentDistance { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int TripTime { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int MedianSpeed { get; set; }
    }

}
