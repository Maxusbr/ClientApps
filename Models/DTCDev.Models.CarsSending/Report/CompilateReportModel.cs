using DTCDev.Models.Date;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Report
{
    [JsonObject]
    public class CompilateReportModel
    {
        public CompilateReportModel()
        {
            SpeedInfo = new List<SpeedReportModel>();
            OBD = new List<OBDParams>();
        }

        [JsonProperty(PropertyName = "A")]
        public string DID { get; set; }

        public int TotalDistance { get; set; }

        public List<OBDParams> OBD { get; set; }

        public List<SpeedReportModel> SpeedInfo { get; set; }

        public class OBDParams
        {
            public OBDParams()
            {
                Date = new DateDataModel();
            }

            public DateDataModel Date {get;set;}

            public string ParamCode { get; set; }

            public int Min { get; set; }

            public int Median { get; set; }

            public int Max { get; set; }
        }

        public class SpeedReportModel
        {
            public SpeedReportModel()
            {
                Date = new DateDataModel();
            }

            public DateDataModel Date { get; set; }

            public int MaxSpeed { get; set; }
        }
    }
}
