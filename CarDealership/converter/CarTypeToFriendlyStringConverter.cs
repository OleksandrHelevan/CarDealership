using System;
using System.Globalization;
using System.Windows.Data;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.converter
{
    public class CarTypeToFriendlyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                Product p when p.Car != null => p.Car.CarType.ToFriendlyString(),
                Car c => c.CarType.ToFriendlyString(),
                CarType ct => ct.ToFriendlyString(),
                _ => string.Empty
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}