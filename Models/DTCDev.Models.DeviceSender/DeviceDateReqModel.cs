using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.Date;

namespace DTCDev.Models.DeviceSender
{
    public class DeviceDateReqModel
    {
        public string DevID { get; set; }

        public DateDataModel DT { get; set; }
    }
}
