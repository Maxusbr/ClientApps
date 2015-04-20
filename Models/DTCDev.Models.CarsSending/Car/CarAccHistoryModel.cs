using DTCDev.Models.Date;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Car
{
    public class CarAccHistoryModel
    {
        public CarAccHistoryModel()
        {
            Data = new List<AccRow>();
        }

            [JsonProperty(PropertyName = "E")]
        public string DevID{get;set;}

            [JsonProperty(PropertyName = "F")]
        public List<AccRow> Data { get; set; }

        public class AccRow
        {
            [JsonProperty(PropertyName = "A")]
            public DateTimeDataModel Date { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public int Z { get; set; }

            [JsonProperty(PropertyName = "B")]
            public int MaxX { get; set; }

            [JsonProperty(PropertyName = "C")]
            public int MaxY { get; set; }

            [JsonProperty(PropertyName = "D")]
            public int MaxZ { get; set; }
        }
    }
}
