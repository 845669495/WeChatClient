using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Dependency;
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
        /// 有聊天被选中
        /// </summary>
        public bool HasChatSelected { [ObservableAsProperty]get; }

        public ChatContentViewModel()
        {
            this.WhenAnyValue(p => p.SelectedChat).Select(p => p != null).ToPropertyEx(this, p => p.HasChatSelected);
        }
    }
}
