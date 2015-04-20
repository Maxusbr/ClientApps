using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;

namespace DTCDev.Models.CarsSending.Report
{
    public class MialageReportModel
    {
        public DateDataModel DT_Start { get; set; }

        public DateDataModel DT_Stop { get; set; }

        public string Number { get; set; }

        public List<DR_Point> Data { get; set; }

        public class DR_Point
        {

            public DateDataModel Dt { get; set; }

            public int Dist { get; set; }

            public int MxSpd { get; set; }

            public TimeModel Str { get; set; }

            public TimeModel Stp { get; set; }
        }
    }
}
