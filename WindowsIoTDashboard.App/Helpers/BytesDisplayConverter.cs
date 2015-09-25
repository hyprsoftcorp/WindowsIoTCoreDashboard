using System;
using Windows.UI.Xaml.Data;

namespace WindowsIoTDashboard.App.Helpers
{
    public sealed class BytesDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var bytes = double.Parse(value.ToString());

            if (bytes < 1048576)
                return String.Format("{0:0.0} KB", bytes / 1024);

            if (bytes >= 1048576 && bytes < 1073741824)
                return String.Format("{0:0.0} MB", bytes / 1048576);

            if (bytes >= 1073741824 && bytes < 1099511627776)
                return String.Format("{0:0.0} GB", bytes / 1073741824);

            if (bytes >= 1099511627776)
                return String.Format("{0:0.0} TB", bytes / 1099511627776);

            return string.Format(String.Format("{0:0.0} bytes", bytes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
