using System.Globalization;
using System.Windows;
using System.Windows.Data;
using CarDealership.enums;

namespace CarDealership.converter
{
    public class PendingVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RequestStatus status)
            {
                return status == RequestStatus.Pending ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
