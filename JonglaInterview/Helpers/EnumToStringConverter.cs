using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace JonglaInterview.Helpers
{
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return DependencyProperty.UnsetValue;
            string enumValue = value.ToString();
            string targetValue = parameter.ToString();
            string outputValue = enumValue.ToString(); // .Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);
            return outputValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return DependencyProperty.UnsetValue;
            string useValue = (string)value;
            string targetValue = parameter.ToString();
            return Enum.Parse(targetType, targetValue);
        }
    }
}
