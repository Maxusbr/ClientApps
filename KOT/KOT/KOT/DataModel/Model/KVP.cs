using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    //Класс-модель для обозначения структуры ключ-значение
    [JsonObject]
    public class KVP
    {
        [JsonProperty(PropertyName = "E")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "F")]
        public string Value { get; set; }
    }
}
