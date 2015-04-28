using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DTCDev.Client.Cars.Service.Convertors
{
    public class DTCConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            if (value.GetType() == typeof(int))
            {
                if ((int)value == 0)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
