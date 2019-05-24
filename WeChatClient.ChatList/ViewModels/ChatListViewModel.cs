using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Attributes;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.ChatList.ViewModels
{
    [ExposeServices(ServiceLifetime.Singleton, typeof(IChatListManager))]   //注册为IChatListManager接口（单例）
    public class ChatListViewModel : ReactiveObject, IChatListManager
    {
        public ObservableCollection<WeChatUser> ChatList { get; private set; } = new ObservableCollection<WeChatUser>();

        [Reactive]
        public WeChatUser SelectedItem { get; set; }

        [Dependency]
        protected IImageDownloadService ImageDownloadService { get; set; }
        [Dependency]
        protected IContactListManager ContactListManager { get; set; }

        public ChatListViewModel(IChatContentManager chatContentManager)
        {
            this.WhenAnyValue(p => p.SelectedItem).Subscribe(p => chatContentManager.SelectedChat = p);
        }

        public void AddChat(params WeChatUser[] chat)
        {
            ChatList.AddRange(chat);
            ImageDownloadService.Add(chat);
        }

        public void SyncMessage(params WeChatMessage[] messages)
        {
            foreach (var msg in messages)
            {
                string userName = msg.IsReceive ? msg.FromUserName : msg.ToUserName;
                var chat = ChatList.FirstOrDefault(p => p.UserName == userName);
                if (chat != null)
                {
                    ChatList.Move(ChatList.IndexOf(chat), 0);
                }
                else
                {
                    //当前列表没有找到
                    chat = ContactListManager.FindContact(userName);
                    if (chat == null)
                        continue;
                    ChatList.Insert(0, chat);
                }
                //消息在当前聊天列表中产生
                if (msg.MsgType != 51)
                    chat.MessageList.Add(msg);
                ImageDownloadService.Add(chat);
            }
        }
    }
}
