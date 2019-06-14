using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Events;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.ChatContent.ViewModels
{
    [ExposeServices(ServiceLifetime.Singleton, typeof(IChatContentManager))]
    public class ChatContentViewModel : ReactiveObject, IChatContentManager
    {
        /// <summary>
        /// 当前选中聊天
        /// </summary>
        [Reactive]
        public WeChatUser SelectedChat { get; set; }
        /// <summary>
        /// 文本框
        /// </summary>
        [Reactive]
        public string Message { get; set; }
        /// <summary>
        /// 发送文本消息命令
        /// </summary>
        public ICommand SendTextMsgCommand { get; private set; }
        /// <summary>
        /// 有聊天被选中
        /// </summary>
        public bool HasChatSelected { [ObservableAsProperty]get; }

        public ChatContentViewModel(IEventAggregator ea)
        {
            SendTextMsgCommand = ReactiveCommand.Create(() =>
            {
                if (string.IsNullOrWhiteSpace(Message))
                    return;
                ea.GetEvent<SendTextMsgEvent>().Publish(Message);
                Message = null;
            });

            var observable = this.WhenAnyValue(p => p.SelectedChat);
            observable.Select(p => p != null).ToPropertyEx(this, p => p.HasChatSelected);
        }
    }
}
