using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Convertors
{
    public class StringToDateTime
    {
        
        public DateTime StringToDate(string date)
        {
            DateTime temp = DateTime.ParseExact(date, "dd.MM.yyyy hh.mm.ss", System.Globalization.DateTimeFormatInfo.CurrentInfo);
            return temp;
        }

        public DateTime StringToDateWithOutTime(string date)
        {
            DateTime temp = DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.DateTimeFormatInfo.CurrentInfo);
            return temp;
        }

        public string DateToString(DateTime dt)
        {
            return dt.ToString("dd.MM.yyyy hh.mm.ss");
        }

        public string DateToStringWithOutTime(DateTime dt)
        {
            return dt.ToString("dd.MM.yyyy");
        }

        public Dictionary<int, int> LastDayOfMonth = new Dictionary<int, int>();

    }
}
