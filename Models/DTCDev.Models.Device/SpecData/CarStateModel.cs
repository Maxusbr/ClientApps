using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DTCDev.Models.Device.SpecData
{
    [DataContract]
    public class CarStateModel
    {

        [DataMember(Name="D")]
        public string DevID { get; set; }

        [DataMember(Name="h")]
        public int hh { get; set; }

        [DataMember(Name="m")]
        public int mm { get; set; }

        [DataMember(Name="s")]
        public int ss { get; set; }

        [DataMember(Name="y")]
        public int yy { get; set; }

        [DataMember(Name="M")]
        public int Mnth { get; set; }

        [DataMember(Name="d")]
        public int dd { get; set; }

        /// <summary>
        /// Скорость * 10
        /// </summary>
        [DataMember(Name="S")]
        public int Spd { get; set; }

        /// <summary>
        /// Latitude * 10000
        /// </summary>
        [DataMember(Name="x")]
        public int Lt { get; set; }

        /// <summary>
        /// Longitude * 10000
        /// </summary>
        [DataMember(Name="z")]
        public int Ln { get; set; }

        /// <summary>
        /// Валидные ли данные. 0 -нет, 1 - да
        /// </summary>
        [DataMember(Name="V")]
        public int Valid { get; set; }

        /// <summary>
        /// Кол-во спутников
        /// </summary>
        [DataMember(Name="a")]
        public int St { get; set; }

        /// <summary>
        /// уровень сигнала сети
        /// </summary>
        [DataMember(Name="G")]
        public int GSM { get; set; }

        /// <summary>
        /// Направление в градусах * 100
        /// </summary>
        [DataMember(Name="q")]
        public int Dir { get; set; }

        [DataMember(Name="w")]
        public int DiscrByte { get; set; }

        [DataMember(Name="e")]
        public List<int> Anlg { get; set; }

        /// <summary>
        /// temperature
        /// </summary>
        /// 
        [DataMember(Name="r")]
        public int Temp { get; set; }

        /// <summary>
        /// Power
        /// </summary>
        [DataMember(Name="p")]
        public int P { get; set; }

        /// <summary>
        /// height * 10
        /// </summary>
        [DataMember(Name="l")]
        public int H { get; set; }
    }
}
