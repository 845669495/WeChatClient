using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WeChatClient.Core.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 是否反转转换源参数值
        /// </summary>
        public bool Inverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = false;
            if (value == null)
                v = false;
            else if (value.GetType() == typeof(bool))
                v = (bool)value;
            else if (value.GetType() == typeof(int))
                v = (int)value > 0;
            else if (value.GetType() == typeof(string))
                v = !string.IsNullOrEmpty((string)value);

            if (Inverse)
                v = !v;
            return v ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
