using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOT.DataModel.Model
{
    public class DateTimeDataModel
    {
        //день
        public int D { get; set; }
        //месяц
        public int M { get; set; }
        //год
        public int Y { get; set; }
        //часы
        public int hh { get; set; }
        //минуты
        public int mm { get; set; }
        //секунды
        public int ss { get; set; }

        public DateTimeDataModel() { }

        public DateTimeDataModel(DateTime dt)
        {
            Y = dt.Year;
            M = dt.Month;
            D = dt.Day;
            hh = dt.Hour;
            mm = dt.Minute;
            ss = dt.Second;
        }

        public DateTime ToDate { get { return new DateTime(Y, M, D); } }
    }
}
