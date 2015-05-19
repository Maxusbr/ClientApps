using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class CarListBaseDataModel
    {
        //Идентификатор устройства, его серийный номер. На основе него в дальнейшем ведется вся обработка входящих сообщений
        [JsonProperty(PropertyName = "0")]
        public string DID { get; set; }
        //Номер или имя автомобиля
        [JsonProperty(PropertyName = "A")]
        public string CarNumber { get; set; }
        //Марка
        [JsonProperty(PropertyName = "B")]
        public string Mark { get; set; }
        //Модель
        [JsonProperty(PropertyName = "C")]
        public string Model { get; set; }
        //Текущий пробег
        [JsonProperty(PropertyName = "E")]
        public int CurrentDistance { get; set; }
        //Пробег до ближайшей работы
        [JsonProperty(PropertyName = "F")]
        public int IncomeDistance { get; set; }
        //название ближайшей работы
        [JsonProperty(PropertyName = "G")]
        public string ServiceName { get; set; }
        //Дней до выполнения работы
        [JsonProperty(PropertyName = "H")]
        public int DaysToService { get; set; }
        //Количество ошибок OBD II
        [JsonProperty(PropertyName = "I")]
        public int DTCErrCount { get; set; }
        //Идентификатор автомобиля
        [JsonProperty(PropertyName = "j")]
        public int ID { get; set; }
    }

}
