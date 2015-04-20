using System;
using System.ComponentModel;
using System.Globalization;

namespace DTCDev.Client.Controls.Map
{
    public class LocationConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return Location.Parse((string)value);
        }
    }

    [TypeConverter(typeof(LocationConverter))]
    public partial class Location
    {
    }
}
