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
        
        [Dependency]
        protected Lazy<IMainManager> MainManager { get; set; }

        public ChatListViewModel(IChatContentManager chatContentManager)
        {
            this.WhenAnyValue(p => p.SelectedItem).Subscribe(p => chatContentManager.SelectedChat = p);
        }

        public void AddChat(params WeChatUser[] chat)
        {
            foreach (var item in chat)
            {
                if (ChatList.Any(p => p.UserName == item.UserName))
                    continue;
                ChatList.Add(item);
                ImageDownloadService.Add(item);
            }          
        }

        public void SyncMessage(params WeChatMessage[] messages)
        {
            var selected = SelectedItem;
            foreach (var msg in messages)
            {
                string userName = msg.IsReceive ? msg.FromUserName : msg.ToUserName;
                var chat = ChatList.FirstOrDefault(p => p.UserName == userName);
                if (chat != null)
                {
                    //消息在当前聊天列表中产生
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
                if (msg.MsgType != 51)  //如果不是系统消息
                {
                    chat.LastMessage = msg.Content;
                    chat.LastShortTime = msg.GroupShortTime;

                    msg.FromUser = msg.IsReceive ? chat : MainManager.Value.WeChatUser;
                    var last = chat.MessageList.LastOrDefault();
                    if (last != null && (msg.CreateDateTime - last.GroupDateTime).Minutes <= 3)
                    {
                        msg.GroupShortTime = last.GroupShortTime;
                        msg.GroupDateTime = last.GroupDateTime;
                    }
                    chat.MessageList.Add(msg);
                }                    
                ImageDownloadService.Add(chat);
            }
            SelectedItem = selected;
        }
    }
}
