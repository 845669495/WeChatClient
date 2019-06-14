using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WeChatClient.Core.Models;

namespace WeChatClient.ChatContent
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dt = null;
            if (item is WeChatMessage obj && container is FrameworkElement fe)
            {
                if (obj.MsgType == 10000)
                    dt = fe.FindResource("SystemMessageTemplate") as DataTemplate;
                else if (obj.IsReceive)
                    dt = fe.FindResource("ReceiveMessageTemplate") as DataTemplate;
                else
                    dt = fe.FindResource("SelfMessageTemplate") as DataTemplate;
            }
            return dt;
        }
    }
}
