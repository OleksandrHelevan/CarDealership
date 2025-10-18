
using System.Globalization;
using System.Windows.Data;
using CarDealership.enums;

namespace CarDealership.converter
{
    public class CarTypeToFriendlyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CarType type)
                return type.ToFriendlyString();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                foreach (CarType type in Enum.GetValues(typeof(CarType)))
                {
                    if (type.ToFriendlyString() == str)
                        return type;
                }                    
            }
            return CarType.Gasoline;
        }
    }
}