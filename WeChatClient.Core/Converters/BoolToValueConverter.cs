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
    /// <summary>
    /// 通用的BoolToXXX转换类，这个类使用Binding对象的ConverterParameter和FallbackValue来实现布尔类型与任意值转换的功能
    /// </summary>
    public class BoolToValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return parameter;
            else
                return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }
    }
}
