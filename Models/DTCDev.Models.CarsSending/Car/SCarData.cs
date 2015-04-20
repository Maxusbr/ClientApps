using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.CarsSending.Navigation;
using DTCDev.Models.Date;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class SCarData
    {
        [JsonProperty(PropertyName = "E")]
        public string ID { get; set; }

        private SNaviData _navigation = new SNaviData();
        [JsonProperty(PropertyName = "A")]
        public SNaviData Navigation
        {
            get { return _navigation; }
            set
            {
                _navigation = value;
            }
        }

        [JsonProperty(PropertyName = "B")]
        public int GSM_Level { get; set; }

        [JsonProperty(PropertyName = "C")]
        public int PowerLevel { get; set; }

        [JsonProperty(PropertyName = "D")]
        public int FuelLevel { get; set; }

        [JsonProperty(PropertyName = "F")]
        public int Temperature { get; set; }

        private DateTimeDataModel _dateUpdate = new DateTimeDataModel();

        [JsonProperty(PropertyName = "G")]
        public DateTimeDataModel DateUpdate
        {
            get { return _dateUpdate; }
            set { _dateUpdate = value; }
        }

        [JsonProperty(PropertyName="I")]
        public List<int> Sensors { get; set; }

        [JsonProperty(PropertyName="J")]
        public int DisByte { get; set; }



        [JsonProperty(PropertyName = "K")]
        public int AcsX { get; set; }

        [JsonProperty(PropertyName = "L")]
        public int AcsY { get; set; }

        [JsonProperty(PropertyName = "M")]
        public int AcsZ { get; set; }

        [JsonProperty(PropertyName = "N")]
        public int AcsXMax { get; set; }

        [JsonProperty(PropertyName = "O")]
        public int AcsYMax { get; set; }

        [JsonProperty(PropertyName = "P")]
        public int AcsZMax { get; set; }
    }
}
