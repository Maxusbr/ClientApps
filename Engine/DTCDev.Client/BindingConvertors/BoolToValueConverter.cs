using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DTCDev.Client.BindingConvertors
{
    public class BoolToValueConverter<T> : IValueConverter
    {

        // geekswithblogs.net/codingbloke/archive/2010/05/28/a-generic-boolean-value-converter.aspx

        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            else
                return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }

    public class BoolToBitmapImageConverter : BoolToValueConverter<BitmapImage> { }

    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility> { }

    public class BoolToBrushConverter : BoolToValueConverter<Brush> { }
}
