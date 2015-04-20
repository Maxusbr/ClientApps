using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.Controls.Map;
using DTCDev.Models.CarsSending.Car;

namespace DTCDev.Client.Cars.Controls.Helpers
{
    public static class StateDateTimeHelper
    {

        public static bool IsParking(CarStateModel curr, ref CarStateModel prev)
        {
            if (curr == prev || prev == null) return false;
            var interval = EqualInterval(curr, prev);
            
            //Location prevloc = new Location
            //{
            //    Latitude = prev.Lt / 10000.0,
            //    Longitude = prev.Ln / 10000.0
            //}, curloc = new Location
            //{
            //    Latitude = curr.Lt / 10000.0,
            //    Longitude = curr.Ln / 10000.0
            //};
            if (curr.Spd >= 6) prev = null;
            if (interval) return true;

            if (curr.Spd < 6 && prev == null)
                prev = curr;
            return false;
        }

        public static bool EqualInterval(CarStateModel curr, CarStateModel prev, int minutes = 10)
        {
            if (curr == null || prev == null) return false;
            var dt = GetTime(curr);
            var times = GetTime(prev);
            if (times > dt) return times - dt >= new TimeSpan(0, minutes, 0);
            return dt - times >= new TimeSpan(0, minutes, 0);
        }

        public static bool EqualInterval(DateTime dt, CarStateModel car, int minutes = 10)
        {
            if (car == null) return false;
            var times = GetTime(car);
            if (times > dt) return times - dt >= new TimeSpan(0, minutes, 0);
            return dt - times >= new TimeSpan(0, minutes, 0);
        }

        public static bool EqualInterval(DateTime dt, DateTime times, int minutes = 10)
        {
            if (times > dt) return times - dt >= new TimeSpan(0, minutes, 0);
            return dt - times >= new TimeSpan(0, minutes, 0);
        }

        public static DateTime GetTime(CarStateModel curr)
        {
            // Исправить при изменение времени сервера
            return new DateTime(curr.yy, curr.Mnth, curr.dd, curr.hh, curr.mm, curr.ss) + (DateTime.Now - DateTime.UtcNow) ;
        }
    }
}
