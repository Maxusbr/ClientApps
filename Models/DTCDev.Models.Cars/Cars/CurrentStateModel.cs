using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.Cars
{
    public class CurrentStateModel
    {
        public string DevID { get; set; }

        public int hh { get; set; }

        public int mm { get; set; }

        public int ss { get; set; }

        public int yy { get; set; }

        public int Mnth { get; set; }

        public int dd { get; set; }

        /// <summary>
        /// Скорость * 100
        /// </summary>
        public int Spd { get; set; }

        /// <summary>
        /// Latitude * 10000
        /// </summary>
        public int Lt { get; set; }

        /// <summary>
        /// Longitude * 10000
        /// </summary>
        public int Ln { get; set; }

        /// <summary>
        /// Валидные ли данные. 0 -нет, 1 - да
        /// </summary>
        public int Valid { get; set; }

        /// <summary>
        /// Кол-во спутников
        /// </summary>
        public int Sat { get; set; }

        /// <summary>
        /// уровень сигнала сети
        /// </summary>
        public int GSM { get; set; }

        /// <summary>
        /// Направление в градусах * 100
        /// </summary>
        public int Dir { get; set; }
    }
}
