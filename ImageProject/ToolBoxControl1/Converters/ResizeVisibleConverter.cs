using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ToolBoxControl.Converters
{
    internal class ResizeVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!Enum.TryParse(value.ToString(), out ItemEditMode res))
                return DependencyProperty.UnsetValue;

            if (res == ItemEditMode.All || res == ItemEditMode.ResizeOnly)
                return Visibility.Visible;

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}