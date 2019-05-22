using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.ChatList.ViewModels
{
    [ExposeServices(ServiceLifetime.Singleton, typeof(IChatListManager))]   //注册为IChatListManager接口（单例）
    public class ChatListViewModel : ReactiveObject, IChatListManager
    {
        public ObservableCollection<WeChatUser> ChatList { get; private set; } = new ObservableCollection<WeChatUser>();

        public void AddChat(params WeChatUser[] chat)
        {
            ChatList.AddRange(chat);
        }
    }
}
