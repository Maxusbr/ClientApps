using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class CarHistoryWorkReport
    {
        //пробег, при котором выполнилась работа
        [JsonProperty(PropertyName = "B")]
        public int Distance { get; set; }
        //комментарий
        [JsonProperty(PropertyName = "C")]
        public string Comment { get; set; }
        //Дата выполнения
        [JsonProperty(PropertyName = "D")]
        public DateDataModel Date { get; set; }
        //Стоимость
        [JsonProperty(PropertyName = "F")]
        public int Cost { get; set; }
        //Название работы
        [JsonProperty(PropertyName = "G")]
        public string WorkName { get; set; }
        //Номер заказ-наряда
        [JsonProperty(PropertyName = "H")]
        public int OrderN { get; set; }
        //Исполнитель
        [JsonProperty(PropertyName = "W")]
        public string Worker { get; set; }
    }

}
