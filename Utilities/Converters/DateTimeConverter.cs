using System;
using System.Globalization;
using System.Windows.Data;

namespace Utilities
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is null) {
                return string.Empty;
            }

            var date = (DateTime)value;
            return date.ToString("dd.MM.yyyy", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return new DateTime();

            DateTime dateTime;
            DateTime.TryParseExact(value.ToString(), "dd.MM.yyyy", culture, DateTimeStyles.None, out dateTime);

            return dateTime;
        }
    }
}
