using Prism.Regions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity.Attributes;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;
using WeChatClient.EmojiCore.Emoji;

namespace WeChatClient.ChatList.ViewModels
{
    [ExposeServices(ServiceLifetime.Singleton, typeof(IChatListManager))]   //注册为IChatListManager接口（单例）
    public class ChatListViewModel : ReactiveObject, IChatListManager, INavigationAware
    {
        public ObservableCollection<WeChatUser> ChatList { get; private set; } = new ObservableCollection<WeChatUser>();

        private WeChatUser _selectedItem;
        //[Reactive]
        public WeChatUser SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedItem, value);
                if (_selectedItem != null)
                    _selectedItem.UnReadCount = 0;
            }
        }

        public ICommand CloseChatCommand { get; }

        [Dependency]
        protected IImageDownloadService ChatImageDownloadService { get; set; }
        [Dependency]
        protected IImageDownloadService MessageImageDownloadService { get; set; }
        [Dependency]
        protected IContactListManager ContactListManager { get; set; }
        [Dependency]
        protected EmojiManager EmojiManager { get; set; }

        //[Dependency]
        //protected Lazy<IMainManager> MainManager { get; set; }

        public ChatListViewModel(IChatContentManager chatContentManager)
        {
            CloseChatCommand = ReactiveCommand.Create<WeChatUser>(CloseChat);

            this.WhenAnyValue(p => p.SelectedItem).Subscribe(p => chatContentManager.SelectedChat = p);
        }

        private void CloseChat(WeChatUser chat)
        {
            ChatList.Remove(chat);
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
            var selected = SelectedItem;
            foreach (var item in chat)
            {
                var local = ChatList.FirstOrDefault(p => p.UserName == item.UserName);
                if (local != null)
                {
                    if (SelectedItem == local)
                        selected = item;

                    item.MessageList.AddRange(local.MessageList);  //将本地聊天的信息拷贝过来
                    item.UnReadCount = local.UnReadCount;
                    ChatList.Remove(local);
                }
                ChatList.Insert(0, item);  //将修改后的聊天放在首位
                ChatImageDownloadService.Add(item);
            }
            SelectedItem = selected;
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
                    ChatImageDownloadService.Add(chat);
                }
                if (msg.MsgType != 51)  //51类型消息不插入消息列表
                {
                    chat.LastMessage = msg.Content?.Trim();
                    chat.LastShortTime = msg.ShortTime;
                    msg.IsRoom = chat.IsRoomContact();

                    if (msg.MsgType != 10000)  //如果不是系统消息
                    {
                        if (msg.IsReceive)  //只有收到消息需要显示名称
                        {
                            //是收到消息
                            if (msg.IsRoom)
                            {
                                //如果是群消息，且不是邀请人进群的消息
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

                            chat.UnReadCount++;
                        }

                        //处理消息头像
                        msg.Uri = msg.FromUserName.GetIconUrl();
                        MessageImageDownloadService.Add(msg);

                        var last = chat.MessageList.Where(p => p.ShowShortTime).LastOrDefault();
                        if (last == null || (msg.CreateDateTime - last.CreateDateTime).Minutes > 3)  //如果新消息之前的消息不存在或者不在三分钟内，则显示时间
                        {
                            msg.ShowShortTime = true;
                        }

                        msg.TbContent = EmojiManager.StringToTextBlock(msg.Content);
                    }
                    chat.MessageList.Add(msg);
                }
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

        public void ChatWithContact(WeChatUser contact)
        {
            var chat = ChatList.FirstOrDefault(p => p.UserName == contact.UserName);
            if(chat == null)
            {
                chat = contact;
                ChatList.Insert(0, chat);
            }
            else
            {
                ChatList.Move(ChatList.IndexOf(chat), 0);
            }
            SelectedItem = chat;
        }

        public WeChatUser[] SearchRoomChat(string searchText)
        {
            return ChatList.Where(p => p.IsRoomContact()).Where(p => p.ShowName?.Contains(searchText) == true || p.ShowPinYin?.Contains(searchText) == true).ToArray();
        }

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            navigationContext.NavigationService.Region.RegionManager.RequestNavigate(WeChatClientConst.ContentRegionName, "ChatContentView");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        #endregion
    }
}
