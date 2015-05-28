using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class OneSpendItem
    {
        //Дата
        [JsonProperty(PropertyName = "D")]
        public DateDataModel Date { get; set; }
        //Сумма
        [JsonProperty(PropertyName = "E")]
        public int Sum { get; set; }
        //Название
        [JsonProperty(PropertyName = "F")]
        public string Name { get; set; }
        //Класс
        [JsonProperty(PropertyName = "G")]
        public int idClass { get; set; }

        [JsonIgnore]
        public string Cost { get { return string.Format("{0} руб.", Sum); } }
    }
}
