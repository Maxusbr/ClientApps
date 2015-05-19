using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class WorkType
    {
        //идентификатор устройства
        [JsonProperty(PropertyName = "0")]
        public string ID { get; set; }
        //название работы
        [JsonProperty(PropertyName = "A")]
        public string Name { get; set; }
        //Сколько километров до выполнения работы
        [JsonProperty(PropertyName = "B")]
        public int DistanceToMake { get; set; }
        //Сколько дней до выполнения работы
        [JsonProperty(PropertyName = "C")]
        public int DaysToMake { get; set; }
        //Тип работы
        [JsonProperty(PropertyName = "D")]
        public int PresentModel { get; set; }
        //периодичность выполнения в км
        [JsonProperty(PropertyName = "E")]
        public int PeriodicDist { get; set; }
        //периодичность выполнения в мес
        [JsonProperty(PropertyName = "F")]
        public int PeriodicTime { get; set; }
        //когда выполнялась в последний раз - дата
        [JsonProperty(PropertyName = "G")]
        public DateDataModel LastMakeTime { get; set; }
        //когда выполнялась в последний раз - пробег
        [JsonProperty(PropertyName = "H")]
        public int LastMakeDist { get; set; }
    }

}
