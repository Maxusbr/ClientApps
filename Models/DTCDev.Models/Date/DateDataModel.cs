using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Date
{
    public class DateDataModel
    {
        public DateDataModel() { }

        public DateDataModel(DateTime dt)
        {
            Y = dt.Year;
            M = dt.Month;
            D = dt.Day;
        }

        public int D { get; set; }

        public int M { get; set; }

        public int Y { get; set; }

        public override string ToString()
        {
            return string.Format("{0:00}.{1:00}.{2:00}", D, M, Y);
        }

        public DateTime ToDate { get { return new DateTime(Y > 0 ? Y : 1, M > 0 ? M : 1, D > 0 ? D : 1); } }
    }
}
