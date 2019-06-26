using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeChatClient.ChatContent.ViewModels;
using WeChatClient.EmojiCore;
using WeChatClient.EmojiCore.Events;

namespace WeChatClient.ChatContent.Views
{
    /// <summary>
    /// ChatContentView.xaml 的交互逻辑
    /// </summary>
    public partial class ChatContentView : UserControl
    {
        public ChatContentView(ChatContentViewModel viewModel, IEventAggregator ea)
        {
            InitializeComponent();
            DataContext = viewModel;

            ea.GetEvent<InputEmojiEvent>().Subscribe(InputEmoji);
        }

        /// <summary>
        /// 输入表情
        /// </summary>
        /// <param name="emoji"></param>
        private void InputEmoji(EmojiModel emoji)
        {
            ContentControl image = new ContentControl
            {
                Content = emoji,
                Width = 20,
                Height = 20,
                Style = (Style)FindResource("FaceImgStyle")
            };
            var container = new InlineUIContainer(image, rtb.CaretPosition) { BaselineAlignment = BaselineAlignment.Bottom };
            rtb.CaretPosition = container.ElementEnd;
            rtb.Focus();
        }
    }
}
