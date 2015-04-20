using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Cars.Cars
{
    public class CarWithWorksModel : DTCDev.Models.Car.Cars.CarModel
    {
        private List<MakedWorks> _works = new List<MakedWorks>();
        /// <summary>
        /// История проведенных работ
        /// </summary>
        public List<MakedWorks> Works
        {
            get { return _works; }
            set { _works = value; }
        }

        /// <summary>
        /// Дневной пробег, используется для рассчета времени до ТО
        /// </summary>
        public int DayDistance { get; set; }


        /// <summary>
        /// Класс описания выполненной работы
        /// </summary>
        public class MakedWorks
        {
            /// <summary>
            /// Идентификатор работы
            /// </summary>
            public string WorkGUID { get; set; }
            
            /// <summary>
            /// пробег, на котором была произведена работа
            /// </summary>
            public int DistanceWork { get; set; }

            /// <summary>
            /// Дата проведения работы
            /// </summary>
            public DateTime DateWork { get; set; }
        }
    }
}
