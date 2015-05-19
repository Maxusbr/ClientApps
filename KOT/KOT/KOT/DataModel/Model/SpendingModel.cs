using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class SpendingModel
    {
        public SpendingModel()
        {
            Spends = new List<OneSpendItem>();
        }
        //общая сумма расходов
        [JsonProperty(PropertyName = "A")]
        public int TotalCost { get; set; }
        //Идентификатор устройства
        [JsonProperty(PropertyName = "B")]
        public string DID { get; set; }
        //Список трат
        [JsonProperty(PropertyName = "C")]
        public List<OneSpendItem> Spends { get; set; }
    }
}
