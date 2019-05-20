using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WeChatClient.Core.Helpers;
using WeChatClient.Core.Http;

namespace WeChatClient.Core.Converters
{
    public sealed class UriToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            try
            {
                byte[] bytes = BaseService.Request(value.ToString(), MethodEnum.GET);
                return ImageHelper.MemoryToImageSourceOther(new MemoryStream(bytes));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
