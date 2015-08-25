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
            OBD = new List<OBDParams>();
        }

        [JsonProperty(PropertyName = "A")]
        public string DID { get; set; }

        public DateDataModel Date {get;set;}

        public int TotalDistance { get; set; }

        public int MaxSpeed { get; set; }
        
        public int ErrorCodes { get; set; }

        public int FuelEnd { get; set; }

        public int TotalFuel { get; set; }

        public List<OBDParams> OBD { get; set; }

        public class OBDParams
        {

            public string ParamCode { get; set; }

            public int Min { get; set; }

            public int Median { get; set; }

            public int Max { get; set; }
        }
    }
}
