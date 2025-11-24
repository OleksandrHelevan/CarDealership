using System.Globalization;
using System.Windows.Data;
using CarDealership.enums;

namespace CarDealership.converter
{
    public class RequestStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RequestStatus status)
            {
                return status switch
                {
                    RequestStatus.Pending => "Очікує",
                    RequestStatus.Approved => "Схвалено",
                    RequestStatus.Rejected => "Відхилено",
                    _ => status.ToString()
                };
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
