using DTCDev.Models.CarBase.CarStatData;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.Shops
{
    [JsonObject]
    public class ItemSkladDataModel: ProductDataModel
    {
        /// <summary>
        /// Подразделение
        /// </summary>
        [JsonProperty(PropertyName = "K")]
        public KVPBase Division { get; set; }
        /// <summary>
        /// закупочная стоимость
        /// </summary>
        [JsonProperty(PropertyName = "L")]
        public double Purchase{ get; set; }
        /// <summary>
        /// Розничная стоимость
        /// </summary>
        [JsonProperty(PropertyName = "M")]
        public double Price { get; set; }
        /// <summary>
        /// количество штук на складе
        /// </summary>
        [JsonProperty(PropertyName = "N")]
        public int Quantity { get; set; }
    }
}
