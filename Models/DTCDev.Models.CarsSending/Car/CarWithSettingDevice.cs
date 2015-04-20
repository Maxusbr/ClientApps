using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    /// <summary>
    /// Класс для хранения модели о том, установлено устройство на машину или нет
    /// </summary>
    [JsonObject]
    public class CarWithSettingDevice
    {
        [JsonProperty(PropertyName = "A")]
        public string DID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int IsPlug { get; set; }
    }
}
