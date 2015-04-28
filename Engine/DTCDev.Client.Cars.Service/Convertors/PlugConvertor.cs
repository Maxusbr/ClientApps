using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DTCDev.Client.Cars.Service.Convertors
{
    public class PlugConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int vol = (int)value;
            if (vol == 0)
                return "Нет";
            else
                return "Установлен";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
