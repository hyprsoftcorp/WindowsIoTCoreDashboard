using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace WindowsIoTDashboard.App.Helpers
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (bool.Parse(parameter.ToString()))
                return (bool)value ? Visibility.Collapsed: Visibility.Visible;
            else
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
