using System.Globalization;

namespace KnowledgeTracker.Converters
{
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class GreaterThanZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                bool result = intValue > 0;

                if (parameter is string param && param.Equals("invert", StringComparison.OrdinalIgnoreCase))
                    return !result;

                return result;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class NullableDateTimeToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                // Treat MinValue or very old dates as "empty"
                if (dt == DateTime.MinValue || dt.Year < 1900)
                    return DateTime.Today;
                return dt;
            }
            return DateTime.Today;  // valor padrão quando null
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
