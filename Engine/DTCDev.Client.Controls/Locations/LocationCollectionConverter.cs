using System;
using System.ComponentModel;
using System.Globalization;

namespace DTCDev.Client.Controls.Map
{
    public class LocationCollectionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return LocationCollection.Parse((string)value);
        }
    }

    [TypeConverter(typeof(LocationCollectionConverter))]
    public partial class LocationCollection
    {
    }
}
