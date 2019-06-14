using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WeChatClient.Core.Converters
{
    public class WxResConverter : IMultiValueConverter, IValueConverter
    {
        /// <summary>
        /// 从微信Web版扒下来的资源
        /// </summary>
        private static readonly BitmapFrame WxRes = BitmapFrame.Create(new Uri("pack://application:,,,/WeChatClient.Core;component/Resources/wx_res.png", UriKind.Absolute));

        #region IMultiValueConverter
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int[] ps = values[0].ToString().Split(' ')[(bool)values[1] ? 0 : 1].Split(',').Select(p => int.Parse(p)).ToArray();
            return GetCroppedBitmap(ps[0], ps[1], ps[2], ps[3]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            int[] ps = value.ToString().Split(',').Select(p => int.Parse(p)).ToArray();
            return GetCroppedBitmap(ps[0], ps[1], ps[2], ps[3]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// 获取一张图片中的一部分
        /// </summary>
        /// <param name="ImgUri">图片路径</param>
        /// <param name="XCoordinate">要截取部分的X坐标</param>
        /// <param name="YCoordinate">要截取部分的Y坐标</param>
        /// <param name="Width">截取的宽度</param>
        /// <param name="Height">截取的高度</param>
        /// <returns></returns>
        private static BitmapSource GetCroppedBitmap(int XCoordinate, int YCoordinate, int Width, int Height)
        {           
            return new CroppedBitmap(WxRes, new Int32Rect(XCoordinate, YCoordinate, Width, Height));
        }
    }
}
