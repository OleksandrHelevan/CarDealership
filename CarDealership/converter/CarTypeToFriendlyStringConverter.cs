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
            if (value is Product product && product.Car != null)
            {
                return product.Car.CarType.ToFriendlyString();
            }
            if (value is CarDealership.enums.CarType carType)
            {
                return carType.ToFriendlyString();
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
