using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarsSending.Report
{
    [JsonObject]
    public class ReportRequestSpeedOveral :ReportRequestBase
    {
        public int SpeedMax { get; set; }
    }
}
