using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class SpendingClassesModel
    {
        [JsonProperty(PropertyName = "A")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string ClassName { get; set; }
    }
}
