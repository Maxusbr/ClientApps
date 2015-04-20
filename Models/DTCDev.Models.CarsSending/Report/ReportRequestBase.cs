using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;

namespace DTCDev.Models.CarsSending.Report
{
    public class ReportRequestBase
    {
        public string DID { get; set; }

        private DateDataModel _start = new DateDataModel();
        public DateDataModel Start
        {
            get { return _start; }
            set { _start = value; }
        }

        private DateDataModel _stop = new DateDataModel();
        public DateDataModel Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }
    }
}
