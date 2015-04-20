using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.Users
{
    [JsonObject]
    public class UserBaseDataModel
    {
        [JsonProperty(PropertyName = "A")]
        public string FIO { get; set; }

        [JsonProperty(PropertyName = "B")]
        public string Login { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "D")]
        public string Position { get; set; }

        [JsonProperty(PropertyName = "E")]
        public int CompanyId { get; set; }

        [JsonProperty(PropertyName = "F")]
        public DateDataModel LastLogin { get; set; }

        [JsonProperty(PropertyName = "G")]
        public List<string> Positions { get; set; }

        [JsonProperty(PropertyName = "H")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "I")]
        public string CompanyAdress { get; set; }

        [JsonProperty(PropertyName = "J")]
        public List<string> Company { get; set; }
    }
}
