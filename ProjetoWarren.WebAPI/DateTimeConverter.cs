using System;
using System.ComponentModel;
using System.Globalization;

namespace ProjetoWarren.WebAPI
{

    public class DateTimeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string)
            {
                if (DateTime.TryParse(((string)value), new CultureInfo("de-DE"), DateTimeStyles.None, out DateTime date)) return date;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

}