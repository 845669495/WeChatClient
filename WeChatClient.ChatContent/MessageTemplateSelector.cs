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
            var fe = container as FrameworkElement;
            var obj = item as WeChatMessage;
            DataTemplate dt = null;
            if (obj != null && fe != null)
            {
                if (obj.IsReceive)
                    dt = fe.FindResource("ReceiveMessageTemplate") as DataTemplate;
                else
                    dt = fe.FindResource("SelfMessageTemplate") as DataTemplate;
            }
            return dt;
        }
    }
}
