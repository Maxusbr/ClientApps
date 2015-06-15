using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class TimeModel
    {
        [JsonProperty(PropertyName = "A")]
        public int H { get; set; }
        [JsonProperty(PropertyName = "B")]
        public int M { get; set; }
        [JsonProperty(PropertyName = "C")]
        public int S { get; set; }

        public override string ToString()
        {
            return string.Format("{0:00}:{1:00}", H, M);
        }

        [JsonIgnore]
        public TimeSpan ToTimeSpan { get { return new TimeSpan(H, M, S); } }
    }
}
