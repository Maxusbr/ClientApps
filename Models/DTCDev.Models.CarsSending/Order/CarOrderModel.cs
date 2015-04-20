using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.Date;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Order
{
    [JsonObject]
    public class OrderModel
    {

        [JsonProperty(PropertyName = "A")]
        public string CarNumber { get; set; }

        [JsonProperty(PropertyName = "B")]
        public DateDataModel DTCreate { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int OrderNumer { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string UserComment { get; set; }

        [JsonProperty(PropertyName = "F")]
        public int Cost { get; set; }
    }

    [JsonObject]
    public class CarOrderModel : OrderModel
    {
        public CarOrderModel()
        {
            Works = new List<WorksInfoDataModel>();
        }

        [JsonProperty(PropertyName = "D")]
        public List<WorksInfoDataModel> Works { get; set; }
    }
}
