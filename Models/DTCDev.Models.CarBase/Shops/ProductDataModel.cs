using DTCDev.Models.CarBase.CarStatData;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.Shops
{
    /// <summary>
    /// Сущность Товар
    /// </summary>
    [JsonObject]
    public class ProductDataModel : KVPBase
    {
        /// <summary>
        /// Артикул
        /// </summary>
        [JsonProperty(PropertyName = "C")]
        public string ArtNo { get; set; }
        /// <summary>
        /// Производитель
        /// </summary>
        [JsonProperty(PropertyName = "D")]
        public KVPBase Vendor { get; set; }
        /// <summary>
        /// Тип товара. Редактируется пользователем, список свободный, предустановлены "запчасти", "дополнительное оборудование"
        /// </summary>
        [JsonProperty(PropertyName = "E")]
        public KVPBase Type { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [JsonProperty(PropertyName = "F")]
        public string Info { get; set; }
        /// <summary>
        /// Фото
        /// </summary>
        [JsonProperty(PropertyName = "G")]
        public string LinkPhoto { get; set; }
        /// <summary>
        /// единица измерения (не редактируется, заданный список - литр, грамм, килограмм, миллилитр, штука)
        /// </summary>
        [JsonProperty(PropertyName = "H")]
        public KVPBase Unit { get; set; }
        /// <summary>
        /// Содержание в сущности единиц (например, 500 мл)
        /// </summary>
        [JsonProperty(PropertyName = "I")]
        public double BaseValue { get; set; }
        /// <summary>
        /// применимость (по дефолту - для всех, либо марка, либо марка/модель)
        /// </summary>
        [JsonProperty(PropertyName = "J")]
        public KVPBase Uses { get; set; }
    }
}
