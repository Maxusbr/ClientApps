using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Service
{
    [JsonObject]
    public class BalanceReportDataModel
    {
        public int CurrentBalance { get; set; }

        public int SMSCount { get; set; }
    }
}
