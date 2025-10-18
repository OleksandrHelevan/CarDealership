using System;
using System.Globalization;
using System.Windows.Data;
using CarDealership.entity;

namespace CarDealership.converter
{
    public class CarTypeToFriendlyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Product product)
            {
                if (product.ElectroCar != null)
                {
                    return "Електричний";
                }
                if (product.GasolineCar != null)
                {
                    return "Бензиновий";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}