using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class DrivingStyle
    {
        //Идентификатор устройства
        [JsonProperty(PropertyName = "X")]
        public string DID { get; set; }
        //Текущий стиль вождения, от 0 до 100% процентов
        [JsonProperty(PropertyName = "A")]
        public int CurrentDrivingStyle { get; set; }
        //Общее значение очков вождения
        [JsonProperty(PropertyName = "B")]
        public int TotalDrivingScore { get; set; }
        //Сегодняшнее значение очков вождения
        [JsonProperty(PropertyName = "C")]
        public int TodayDrivingScore { get; set; }
        //Текущая экологичность вождения, от 0 до 100%
        [JsonProperty(PropertyName = "D")]
        public int CurrentEcoStyle { get; set; }
        //общий показатель экологичности
        [JsonProperty(PropertyName = "E")]
        public int TotalEcoScore { get; set; }
        //Дневной показатель экологичности
        [JsonProperty(PropertyName = "F")]
        public int TodayEcoScore { get; set; }
    }
}
