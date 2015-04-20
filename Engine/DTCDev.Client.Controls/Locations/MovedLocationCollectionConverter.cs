using System;
using System.ComponentModel;
using System.Globalization;

namespace DTCDev.Client.Controls.Map
{
    public class MovedLocationCollectionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return MovedLocationCollection.Parse((string)value);
        }
    }

    [TypeConverter(typeof(MovedLocationCollectionConverter))]
    public partial class MovedLocationCollection
    {
    }
}
