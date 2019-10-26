using System;
using System.Globalization;
using System.Windows.Data;

namespace PedroLamas.WP7.CTTObjectos.Helpers
{
    public class ListViewDateTimeConverter : IValueConverter
    {
        private readonly static Microsoft.Phone.Controls.ListViewDateTimeConverter _listViewDateTimeConverter = new Microsoft.Phone.Controls.ListViewDateTimeConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            return _listViewDateTimeConverter.Convert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}