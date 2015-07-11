using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Date
{
    public class DateTimeDataModel
    {
        public int D { get; set; }

        public int M { get; set; }

        public int Y { get; set; }

        public int hh { get; set; }

        public int mm { get; set; }

        public int ss { get; set; }

        public override string ToString()
        {
            return string.Format("{0:00}.{1:00}.{2} {3}:{4:00}:{5:00}", D, M, Y, hh, mm, ss);
        }

        public DateTime ToDateTime()
        {
            return new DateTime(Y != 0 ? Y : 2001, M != 0 ? M : 1, D != 0 ? D : 1, hh, mm, ss);
        }

        public TimeSpan ToTime()
        {
            return new TimeSpan(hh, mm, ss);
        }

        public DateTimeDataModel() { }

        public DateTimeDataModel(DateTime date)
        {
            Y = date.Year;
            M = date.Month;
            D = date.Day;
            hh = date.Hour;
            mm = date.Minute;
            ss = date.Second;
        }
    }
}
