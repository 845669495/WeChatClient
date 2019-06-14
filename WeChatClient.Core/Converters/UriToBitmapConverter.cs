using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WeChatClient.Core.Helpers;
using WeChatClient.Core.Http;

namespace WeChatClient.Core.Converters
{
    public sealed class UriToBitmapConverter : IValueConverter  //这种方式还是会很卡，弃用（采用后台队列下载图片）
    {
        private static readonly Dictionary<string, ImageSource> ImageCache = new Dictionary<string, ImageSource>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string uri = value.ToString();
            if (!ImageCache.TryGetValue(uri, out ImageSource res))
            {
                try
                {
                    byte[] bytes = BaseService.Request(uri, MethodEnum.GET);
                    res = ImageHelper.MemoryToImageSourceOther(new MemoryStream(bytes));
                    ImageCache.Add(uri, res);
                }
                catch (Exception)
                {
                }
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
