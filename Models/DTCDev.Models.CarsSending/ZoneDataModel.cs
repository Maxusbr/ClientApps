using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending
{
    [JsonObject]
    public class ZoneDataModel
    {
        [JsonProperty(PropertyName="0")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "A")]
        public string Name { get; set; }

        private List<ZonePoint> _points = new List<ZonePoint>();

        [JsonProperty(PropertyName = "B")]
        public List<ZonePoint> Points
        {
            get { return _points; }
            set { _points = value; }
        }

        [JsonProperty(PropertyName="C")]
        public int IsPublic { get; set; }
    }

    [JsonObject]
    public class ZonePoint
    {
        [JsonProperty(PropertyName = "A")]
        public int Latitude { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int Longitude { get; set; }
    }
}
