using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WindowsIoTDashboard.App.Helpers
{
    public class LevelToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new Thickness((int)value * 15, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
