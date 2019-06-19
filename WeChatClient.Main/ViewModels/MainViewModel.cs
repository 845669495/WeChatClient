using Newtonsoft.Json.Linq;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Unity.Attributes;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Events;
using WeChatClient.Core.Helpers;
using WeChatClient.Core.Http;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.Main.ViewModels
{
    [ExposeServices(ServiceLifetime.Singleton,typeof(IMainManager))]
    public class MainViewModel : ReactiveObject, IMainManager
    {
        private WeChatService wcs = new WeChatService();

        private readonly IRegionManager _regionManager;

        [Dependency]
        protected IContactListManager ContactListManager { get; set; }
        [Dependency]  //属性注入，相比构造注入使用比较方便
        protected IChatListManager ChatListManager { get; set; }
        [Dependency]
        protected IChatContentManager ChatContentManager { get; set; }
        [Dependency]
        protected IImageDownloadService ImageDownloadService { get; set; }

        [Reactive]
        public WeChatUser WeChatUser { get; private set; }

        [Reactive]
        public bool ChatNavChecked { get; set; } = true;

        public ICommand LoadedCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand LoginoutCommand { get; }

        public MainViewModel(IRegionManager regionManager, IEventAggregator ea)
        {
            _regionManager = regionManager;
            LoadedCommand = ReactiveCommand.CreateFromTask(InitAsync);
            NavigateCommand = ReactiveCommand.Create<string>(Navigate);
            LoginoutCommand = ReactiveCommand.Create(Loginout);

            ea.GetEvent<SendTextMsgEvent>().Subscribe(SendTextMsg);
            ea.GetEvent<ChatWithContactEvent>().Subscribe(ChatWithContact);
        }

        private void Loginout()
        {
            //wcs.Loginout();  //调用登出接口，移动端没有收到登出的通知，目前还不知道原因
            Application.Current.Shutdown();
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate(WeChatClientConst.NavRegionName, navigatePath);
        }

        /// <summary>
        /// 异步初始化数据
        /// </summary>
        /// <returns></returns>
        private async Task InitAsync()
        {
            var list = await Task.Run(() =>
            {
                JObject init_result = wcs.WeChatInit();
                WeChatUser = JObjectToUser(init_result["User"]);
                ImageDownloadService.Add(WeChatUser);
                return init_result["ContactList"].Select(contact=> JObjectToUser(contact));
            });
            //将数据传输到聊天列表组件
            ChatListManager.AddChat(list.Distinct(new WeChatUserComparer()).ToArray());

            //开启微信状态通知
            await Task.Run(() =>
            {
                wcs.WxStatusNotify(WeChatUser.UserName);
            });

            //加载通讯录
            await LoadAllContact();
            //加载群组成员
            await UpdateInitGroupMember(list.Where(p => p.IsRoomContact()).Select(p => p.UserName).Distinct().ToArray());

            StartWeChatSyncTask();
        }

        /// <summary>
        /// 加载所有通讯录
        /// </summary>
        /// <returns></returns>
        private async Task LoadAllContact()
        {
            var list = await Task.Run(() =>
            {
                //取到通讯录，过滤公众号，然后分组
                JObject contact_result = wcs.GetContact();
                return contact_result["MemberList"]
                .Select(contact => JObjectToUser(contact)).OrderBy(p => p.StartChar).ToArray();
            });

            //将数据传输到通讯录组件
            ContactListManager.AddContact(list);
        }

        /// <summary>
        /// 加载初始化的群组聊天成员（这里很奇怪，微信初始化接口返回的群组成员信息不全，没有昵称）
        /// </summary>
        /// <returns></returns>
        private async Task UpdateInitGroupMember(string[] userNames)
        {
            var list = await Task.Run(() =>
            {
                JObject contact_result = wcs.WxBatchGetContact(userNames);
                return contact_result["ContactList"].Select(contact => JObjectToUser(contact)).ToArray();
            });

            //将初始化的群组聊天成员传输到聊天列表组件
            ChatListManager.UpdateInitGroupMember(list);
        }

        private WeChatUser JObjectToUser(JToken jObject)
        {
            WeChatUser user = jObject.ToObject<WeChatUser>();

            user.HeadImgUrl = user.UserName.GetIconUrl();
            user.ChatNotifyClose = user.IsChatNotifyClose();
            user.StartChar = user.GetStartChar();
            //user.MemberList = jObject["MemberList"].Select(p => p.ToObject<ChatRoomMember>()).ToList();
            if (string.IsNullOrEmpty(user.NickName) && user.MemberList != null)
                user.NickName = string.Join(",", user.MemberList.Select(p => p.NickName));

            return user;
        }

        private WeChatMessage JObjectToMessage(JToken jObject)
        {
            WeChatMessage message = jObject.ToObject<WeChatMessage>();
            //message.Content = message.MsgType == 1 ? message.Content : "请在其他设备上查看消息";//只接受文本消息
            message.CreateDateTime = message.CreateTime.ToTime();
            message.ShortTime = message.CreateDateTime.ToString("HH:mm");
            message.IsReceive = message.ToUserName == WeChatUser.UserName;
            return message;
        }

        /// <summary>
        /// 开启微信同步任务
        /// </summary>
        private void StartWeChatSyncTask()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    //同步检查
                    string sync_flag = wcs.WeChatSyncCheck();
                    if (sync_flag == null)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    //这里应该判断sync_flag中Selector的值
                    else
                    {
                        JObject sync_result = wcs.WeChatSync();//进行同步
                        if (sync_result != null)
                        {
                            if (sync_result["DelContactCount"] != null && sync_result["DelContactCount"].ToString() != "0")  //删除联系人
                            {
                                var delContacts = sync_result["DelContactList"].Select(p => p.Value<string>("UserName")).ToArray();
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    //同时删除聊天和联系人
                                    ChatListManager.DelChat(delContacts);
                                    ContactListManager.DelContact(delContacts);
                                }));
                            }
                            if (sync_result["ModContactCount"] != null && sync_result["ModContactCount"].ToString() != "0")  //添加或者修改联系人
                            {
                                var modContactList = sync_result["ModContactList"].Select(p => JObjectToUser(p)).ToArray();
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    ChatListManager.ModChat(modContactList);
                                    ContactListManager.ModContact(modContactList);                                
                                }));
                            }
                            if (sync_result["AddMsgCount"] != null && sync_result["AddMsgCount"].ToString() != "0")  //新消息
                            {
                                var messageList = sync_result["AddMsgList"].Select(p => JObjectToMessage(p));

                                var loadMore = messageList.FirstOrDefault(p => p.IsLoadMoreChats);
                                if (loadMore != null)
                                {
                                    //加载更多聊天列表
                                    string[] userNames = loadMore.StatusNotifyUserName.Split(',').Where(p => !ChatListManager.Contains(p)).ToArray();
                                    LoadMoreChats(userNames);
                                }
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    //同步消息
                                    ChatListManager.SyncMessage(messageList.ToArray());
                                }));      
                            }
                        }
                    }
                    Thread.Sleep(100);
                }
            });
        }

        /// <summary>
        /// 启动新线程加载更多聊天列表
        /// </summary>
        /// <param name="userNames"></param>
        private void LoadMoreChats(string[] userNames)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < Math.Ceiling(userNames.Length / 50.0); i++)  //每次最多查询50条数据
                {
                    JObject contact_result = wcs.WxBatchGetContact(userNames.Skip(i * 50).Take(50).ToArray());
                    var chatList = contact_result["ContactList"].Select(contact => JObjectToUser(contact)).ToArray();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ChatListManager.AddChat(chatList);
                    }));
                }
            });
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="msg"></param>
        private void SendTextMsg(string msg)
        {
            var selectedChat = ChatContentManager.SelectedChat;

            WeChatMessage weChatMessage = new WeChatMessage
            {
                Content = msg,
                CreateDateTime = DateTime.Now,
                FromUserName = WeChatUser.UserName,
                MsgType = 1,
                ToUserName = selectedChat.UserName,
                ShortTime = DateTime.Now.ToString("HH:mm"),
                IsReceive = false
            };

            ChatListManager.SyncMessage(weChatMessage);

            wcs.SendMsg(msg, WeChatUser.UserName, selectedChat.UserName, 1);
        }

        private void ChatWithContact(WeChatUser contact)
        {
            ChatListManager.ChatWithContact(contact);

            //导航到聊天列表
            NavigateCommand.Execute("ChatListView");
            ChatNavChecked = true;
        }
    }
}
