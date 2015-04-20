using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Client.Window
{
    [JsonObject]
    public class SettingsModel
    {

        [JsonProperty(PropertyName = "1")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "2")]
        public double Width { get; set; }

        [JsonProperty(PropertyName = "3")]
        public double Height { get; set; }

        [JsonProperty(PropertyName = "4")]
        public double Left { get; set; }

        [JsonProperty(PropertyName = "5")]
        public double Top { get; set; }

        [JsonProperty(PropertyName = "6")]
        public bool Maximize { get; set; }

        [JsonProperty(PropertyName = "7")]
        public bool Minimized { get; set; }

        [JsonProperty(PropertyName = "8")]
        public string LinkId { get; set; }

    }
}
