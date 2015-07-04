using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using DTCDev.Models.User;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Order
{
    [JsonObject]
    public class CarOrderPostModel : CarOrderModel
    {
        /// <summary>
        /// перемещение в заказ-наряды
        /// </summary>
        [JsonProperty(PropertyName = "G")]
        public int InUse { get; set; }

        [JsonProperty(PropertyName = "H")]
        public int PostId { get; set; }

        [JsonProperty(PropertyName = "I")]
        public UserLightModel User { get; set; }
        /// <summary>
        /// планируемая дата начала работ
        /// </summary>
        [JsonProperty(PropertyName = "J")]
        public DateDataModel DateWork { get; set; }
    }
}
