using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    class PlacesModel
    {
        //Идентификатор
        [JsonProperty(PropertyName = "A")]
        public int ID { get; set; }
        //Название
        [JsonProperty(PropertyName = "B")]
        public string Name { get; set; }
        //Адрес
        [JsonProperty(PropertyName = "C")]
        public string Adress { get; set; }
        //Широта (поделить на 10000)
        [JsonProperty(PropertyName = "D")]
        public int Latitude { get; set; }
        //Долгота
        [JsonProperty(PropertyName = "E")]
        public int Longitude { get; set; }
        //Минимальный ценник
        [JsonProperty(PropertyName = "F")]
        public int MinPrice { get; set; }
        //Рейтинг (от 0 до 100)
        [JsonProperty(PropertyName = "G")]
        public int Score { get; set; }
        //Количество комментариев
        [JsonProperty(PropertyName = "H")]
        public int Comments { get; set; }
        //Категория
        [JsonProperty(PropertyName = "I")]
        public int idCategory { get; set; }
    }
}
