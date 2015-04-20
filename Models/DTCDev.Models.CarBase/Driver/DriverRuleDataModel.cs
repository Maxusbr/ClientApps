using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.Driver
{
    [JsonObject]
    public class DriverRuleDataModel
    {
        /// <summary>
        /// Идентификатор требования
        /// </summary>
        [JsonProperty(PropertyName = "A")]
        public int RuleID { get; set; }

        /// <summary>
        /// Идентификатор водителя
        /// </summary>
        [JsonProperty(PropertyName = "B")]
        public int DriverID { get; set; }

        /// <summary>
        /// Статус - 0 если не соответствует, 1 если соответствует
        /// </summary>
        [JsonProperty(PropertyName = "C")]
        public int Status { get; set; }
    }
}
