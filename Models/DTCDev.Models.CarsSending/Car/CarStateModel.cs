using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Car
{
    [JsonObject]
    public class CarStateModel
    {

        [JsonProperty(PropertyName = "D")]
        public string DevID { get; set; }

        [JsonProperty(PropertyName = "h")]
        public int hh { get; set; }

        [JsonProperty(PropertyName = "m")]
        public int mm { get; set; }

        [JsonProperty(PropertyName = "s")]
        public int ss { get; set; }

        [JsonProperty(PropertyName = "y")]
        public int yy { get; set; }

        [JsonProperty(PropertyName = "M")]
        public int Mnth { get; set; }

        [JsonProperty(PropertyName = "d")]
        public int dd { get; set; }

        /// <summary>
        /// Скорость * 10
        /// </summary>
        [JsonProperty(PropertyName = "S")]
        public int Spd { get; set; }

        /// <summary>
        /// Latitude * 10000
        /// </summary>
        [JsonProperty(PropertyName = "x")]
        public int Lt { get; set; }

        /// <summary>
        /// Longitude * 10000
        /// </summary>
        [JsonProperty(PropertyName = "z")]
        public int Ln { get; set; }

        /// <summary>
        /// Валидные ли данные. 0 -нет, 1 - да
        /// </summary>
        [JsonProperty(PropertyName = "V")]
        public int Valid { get; set; }

        /// <summary>
        /// Кол-во спутников
        /// </summary>
        [JsonProperty(PropertyName = "a")]
        public int St { get; set; }

        /// <summary>
        /// уровень сигнала сети
        /// </summary>
        [JsonProperty(PropertyName = "G")]
        public int GSM { get; set; }

        /// <summary>
        /// Направление в градусах * 100
        /// </summary>
        [JsonProperty(PropertyName = "q")]
        public int Dir { get; set; }

        [JsonProperty(PropertyName = "w")]
        public int DiscrByte { get; set; }

        [JsonProperty(PropertyName = "e")]
        public List<int> Anlg { get; set; }

        /// <summary>
        /// temperature
        /// </summary>
        /// 
        [JsonProperty(PropertyName = "r")]
        public int Temp { get; set; }

        /// <summary>
        /// Power
        /// </summary>
        [JsonProperty(PropertyName = "p")]
        public int P { get; set; }

        /// <summary>
        /// height * 10
        /// </summary>
        [JsonProperty(PropertyName = "l")]
        public int H { get; set; }

        private int _seconds = 0;

        [JsonIgnore]
        public int Seconds
        {
            get
            {
                if (_seconds == 0)
                {
                    TimeSpan ts = new TimeSpan(hh, mm, ss);
                    _seconds = (int)ts.TotalSeconds;
                }
                return _seconds;
            }
        }
    }
}
