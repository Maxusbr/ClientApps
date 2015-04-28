using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DTCDev.Client.Cars.Service.Convertors
{
    public class OnLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int vol = (int)value;
            if (vol == -1)
                return "Н.Д.";
            if (vol < 10)
                return "В сети";
            else
            {
                int hours = vol / 60;
                if (hours > 24)
                {
                    int days = hours / 24;
                    if (days > 1)
                        if (days < 90)
                            return days.ToString() + "дней назад";
                        else
                            return "Очень давно";
                    else
                        return hours.ToString() + " час. назад";
                }
                else
                    if (hours > 2)
                        return hours.ToString() + " час. назад";
                    else
                        return vol.ToString() + "мин. назад";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return DateTime.Now;
        }
    }
}
