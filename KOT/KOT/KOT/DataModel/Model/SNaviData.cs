using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class SNaviData
    {
        [JsonProperty(PropertyName = "A")]
        public int Latitude { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int Longitude { get; set; }

        //кол-во спутников
        [JsonProperty(PropertyName = "C")]
        public int Sattelites { get; set; }

        //Скорость
        [JsonProperty(PropertyName = "D")]
        public int Speed { get; set; }

        //вектор направления
        [JsonProperty(PropertyName = "E")]
        public int Direction { get; set; }
    }
}
