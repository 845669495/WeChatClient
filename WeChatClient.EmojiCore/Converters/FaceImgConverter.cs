using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WeChatClient.EmojiCore.Converters
{
    public class FaceImgConverter : IValueConverter
    {
        private static readonly BitmapFrame emoji_face_res = BitmapFrame.Create(new Uri("pack://application:,,,/WeChatClient.EmojiCore;component/Resources/6AfH8-r.png", UriKind.Absolute));
        private static readonly BitmapFrame qq_face_res = BitmapFrame.Create(new Uri("pack://application:,,,/WeChatClient.EmojiCore;component/Resources/15BPafa.png", UriKind.Absolute));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EmojiModel model = (EmojiModel)value;
            int size = model.Size == EmojiSize.Medium ? 28 : 20;
            BitmapFrame res = null;
            if (model.Size == EmojiSize.Medium)
            {

                if (model.Type == EmojiType.Emoji)
                {
                    res = emoji_face_res;
                }
                else
                {
                    res = qq_face_res;
                }
            }

            if (res == null)
                return null;

            return new CroppedBitmap(res, new Int32Rect((int)model.Position.X , (int)model.Position.Y, size, size));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
