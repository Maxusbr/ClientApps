using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.Date;
using DTCDev.Models.User;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class TestDriveModel
    {
        [JsonProperty(PropertyName = "A")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "B")]
        public CarListBaseDataModel Car { get; set; }

        [JsonProperty(PropertyName = "C")]
        public TestDriveUserModel User { get; set; }

        [JsonProperty(PropertyName = "D")]
        public DateTimeDataModel Date { get; set; }
    }
}
