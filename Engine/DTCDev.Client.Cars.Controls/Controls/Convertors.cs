using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DTCDev.Models.Date;

namespace DTCDev.Client.Cars.Controls.Controls
{
    public class IntToDateTimeConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to the display state of an element.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            else
            {
                var i = (int)value;
                int d = i / 86400;
                int h = (i / 3600);
                int m = ((int)(i / 60)) - h * 60;
                int s = i - h * 3600 - m * 60;

                var res = "";

                if (d > 0)
                    res += d.ToString() + " дней, ";

                if (h == 0) res = "00:";
                else
                {
                    if (h < 10) res = "0" + h + ":";
                    else
                    {
                        res = h.ToString() + ":";
                    }
                }
                if (m == 0) res += "00:";
                else
                {
                    if (m < 10) res += "0" + m + ":";
                    else
                    {
                        res += m.ToString() + ":";
                    }
                }
                if (s == 0) res += "00";
                else
                {
                    if (s < 10) res += "0" + s;
                    else
                    {
                        res += s.ToString();
                    }
                }

                return res;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return null;
        }

    }

    public class DateModelToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "не указано";
            else
            {
                var i = (DateDataModel)value;
                StringBuilder sb = new StringBuilder();
                sb.Append(i.D);
                sb.Append('.');
                sb.Append(i.M);
                sb.Append('.');
                sb.Append(i.Y);
                return sb.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class ReportPointTimeToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "не указано";
            else
            {
                var i = (TimeModel)value;
                StringBuilder sb = new StringBuilder();
                sb.Append(i.H);
                sb.Append(':');
                if (i.M > 9)
                    sb.Append(i.M);
                else
                {
                    sb.Append("0");
                    sb.Append(i.M);
                }
                return sb.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class SpeedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(int))
                return 0;
            int data = (int)value;
            return data / 10.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TimeToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(int))
                return "";
            int data = (int)value;
            TimeSpan ts = TimeSpan.FromMinutes(data);
            if (ts.Minutes < 10)
                return ts.Hours.ToString() + ":0" + ts.Minutes.ToString();
            else
                return ts.Hours.ToString() + ":" + ts.Minutes.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class DistConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(int))
                return "Err";
            int data = (int)value;
            double d = (double)data / 1000.0;
            return Math.Round(d, 2).ToString() + " км";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
