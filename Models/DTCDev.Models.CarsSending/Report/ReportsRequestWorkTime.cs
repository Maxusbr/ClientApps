using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;

namespace DTCDev.Models.CarsSending.Report
{
    public class ReportsRequestWorkTime : ReportRequestBase
    {
        public TimeModel StartWork { get; set; }

        public TimeModel StopWork { get; set; }
    }
}
