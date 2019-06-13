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
        protected IImageDownloadService ChatImageDownloadService { get; set; }
        [Dependency]
        protected IImageDownloadService MessageImageDownloadService { get; set; }
        [Dependency]
        protected IContactListManager ContactListManager { get; set; }
        
        //[Dependency]
        //protected Lazy<IMainManager> MainManager { get; set; }

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
                ChatImageDownloadService.Add(item);
            }          
        }

        public void ModChat(params WeChatUser[] chat)
        {
            foreach (var item in chat)
            {
                var local = ChatList.FirstOrDefault(p => p.UserName == item.UserName);
                if (local != null)
                {
                    item.MessageList.AddRange(local.MessageList);  //将本地聊天的信息拷贝过来
                    ChatList.Remove(local);
                }
                ChatList.Insert(0, item);  //将修改后的聊天放在首位
                ChatImageDownloadService.Add(item);
            }
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool Contains(string userName)
        {
            return ChatList.Any(p => p.UserName == userName);
        }

        public void SyncMessage(params WeChatMessage[] messages)
        {
            var selected = SelectedItem;
            foreach (var msg in messages)
            {
                if (msg.IsLoadMoreChats)
                    continue;
                string chatUserName = msg.IsReceive ? msg.FromUserName : msg.ToUserName;
                var chat = ChatList.FirstOrDefault(p => p.UserName == chatUserName);
                if (chat != null)
                {
                    //消息在当前聊天列表中产生
                    ChatList.Move(ChatList.IndexOf(chat), 0);
                }
                else
                {
                    //当前列表没有找到
                    chat = ContactListManager.FindContact(chatUserName);
                    if (chat == null)
                        continue;
                    ChatList.Insert(0, chat);
                }
                if (msg.MsgType != 51)  //如果不是系统消息
                {
                    chat.LastMessage = msg.Content;
                    chat.LastShortTime = msg.GroupShortTime;
                    msg.IsRoom = chat.IsRoomContact();

                    if (msg.IsReceive)  //只有收到消息需要显示名称
                    {
                        //是收到消息
                        if (msg.IsRoom)
                        {
                            //如果是群消息
                            var member = chat.MemberList.FirstOrDefault(p => msg.Content.StartsWith(p.UserName));
                            if (member == null)
                                continue;
                            msg.Content = msg.Content.Replace(member.UserName + ":<br/>", "");
                            chat.LastMessage = member.ShowName + ":" + msg.Content;
                            msg.FromUserName = member.UserName;
                            msg.FromUserShowName = member.ShowName;
                        }
                        else
                        {
                            //不是群消息则显示对方显示名称
                            msg.FromUserShowName = chat.ShowName;
                        }
                    }

                    msg.Uri = msg.FromUserName.GetIconUrl();
                    MessageImageDownloadService.Add(msg);

                    var last = chat.MessageList.LastOrDefault();
                    if (last != null && (msg.CreateDateTime - last.GroupDateTime).Minutes <= 3)
                    {
                        msg.GroupShortTime = last.GroupShortTime;
                        msg.GroupDateTime = last.GroupDateTime;
                    }
                    chat.MessageList.Add(msg);
                }                    
                ChatImageDownloadService.Add(chat);
            }
            SelectedItem = selected;
        }

        public void UpdateInitGroupMember(params WeChatUser[] groupChats)
        {
            foreach (var item in groupChats)
            {
                var chat = ChatList.FirstOrDefault(p => p.UserName == item.UserName);
                if (chat == null) continue;
                chat.MemberList = item.MemberList;
            }
        }

        public void DelChat(params string[] userNames)
        {
            foreach (var item in userNames)
            {
                var user = ChatList.FirstOrDefault(p => p.UserName == item);
                if (user != null)
                {
                    ChatList.Remove(user);
                }
            }
        }
    }
}
