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
        //Идентификатор устройства
        [JsonProperty(PropertyName = "X")]
        public string DID { get; set; }
        //Текущий пробег от сброса, км
        [JsonProperty(PropertyName = "A")]
        public int CurrentDistance { get; set; }
        //Время от сброса в минутах
        [JsonProperty(PropertyName = "B")]
        public int TripTime { get; set; }
        //средняя скорость, км/ч
        [JsonProperty(PropertyName = "C")]
        public int MedianSpeed { get; set; }
        //Далее – аналогично, но для параметров «от запуска двигателя»
        [JsonProperty(PropertyName = "D")]
        public int EngCurrentDistance { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int EngTripTime { get; set; }

        [JsonProperty(PropertyName = "X")]
        public int EngMedianSpeed { get; set; }
    }

}
