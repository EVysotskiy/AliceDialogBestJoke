using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Logic.Converters;

public class StringIdConverter<T> : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        var stringValue = value as string;
        if (stringValue != null)
        {
            return Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[] { stringValue }, CultureInfo.InvariantCulture);
        }

        return base.ConvertFrom(context, culture, value);
    }
}