using System;
using System.Globalization;
using System.Windows.Data;

namespace Utilities
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return 0;

            string val = value.ToString();

            val = val.Replace('.', ',');
            return Decimal.Parse(val);
        }
    }
}
