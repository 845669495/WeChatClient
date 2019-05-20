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
    [ExposeServices(ServiceLifetime.Singleton, typeof(IChatListManager))]
    public class ChatListViewModel : ReactiveObject, IChatListManager
    {
        public ObservableCollection<WeChatUser> ChatUserList { get; private set; } = new ObservableCollection<WeChatUser>();

        public void AddChatUser(params WeChatUser[] chat)
        {
            ChatUserList.AddRange(chat);
        }
    }
}
