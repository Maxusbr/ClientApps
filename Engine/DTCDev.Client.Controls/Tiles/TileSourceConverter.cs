using System;
using System.ComponentModel;
using System.Globalization;

namespace DTCDev.Client.Controls.Map
{
    public class TileSourceConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new TileSource(value as string);
        }
    }

    [TypeConverter(typeof(TileSourceConverter))]
    public partial class TileSource
    {
    }
}
