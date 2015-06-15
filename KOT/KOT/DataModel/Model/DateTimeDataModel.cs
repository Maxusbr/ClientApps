using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class DateTimeDataModel
    {
        //день
        public int D { get; set; }
        //месяц
        public int M { get; set; }
        //год
        public int Y { get; set; }
        //часы
        public int hh { get; set; }
        //минуты
        public int mm { get; set; }
        //секунды
        public int ss { get; set; }

        public DateTimeDataModel() { }

        public DateTimeDataModel(DateTime dt)
        {
            Y = dt.Year;
            M = dt.Month;
            D = dt.Day;
            hh = dt.Hour;
            mm = dt.Minute;
            ss = dt.Second;
        }
        [JsonIgnore]
        public DateTime ToDate { get { return new DateTime(Y, M, D); } }
    }
}
