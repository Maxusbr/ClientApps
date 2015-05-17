using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class CarAlarmsState
    {
        //Идентификатор устройства
        [JsonProperty(PropertyName = "A")]
        public string DID { get; set; }
        //Уровень удара – 0 – нет, 1- слабое, 2 – среднее, 3 - сильное
        [JsonProperty(PropertyName = "B")]
        public int AlarmLevel { get; set; }
        //Включен ли свет 0 – нет, 1 - да
        [JsonProperty(PropertyName = "C")]
        public int IsLightsOn { get; set; }
        //Заперта ли дверь 0 – нет, 1 - да
        [JsonProperty(PropertyName = "D")]
        public int IsDoorClosed { get; set; }
    }
}
