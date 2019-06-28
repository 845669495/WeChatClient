using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WeChatClient.EmojiCore.Emoji;

namespace WeChatClient.EmojiCore.Converters
{
    public class StringToTextBlockConverter : IValueConverter
    {
        public TextTrimming TextTrimming { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EmojiManager.Instance.StringToTextBlock(value?.ToString(), TextTrimming);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
