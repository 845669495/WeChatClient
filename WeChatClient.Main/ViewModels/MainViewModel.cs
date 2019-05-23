using Newtonsoft.Json.Linq;
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
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Unity.Attributes;
using WeChatClient.Core.Http;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.Main.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private WeChatService wcs = new WeChatService();

        private readonly IRegionManager _regionManager;

        [Dependency]
        protected IContactListManager ContactListManager { get; set; }
        [Dependency]  //属性注入，相比构造注入使用比较方便
        protected IChatListManager ChatListManager { get; set; }
        [Dependency]
        protected IImageDownloadService ImageDownloadService { get; set; }

        [Reactive]
        public WeChatUser WeChatUser { get; private set; }

        public ICommand LoadedCommand { get; }

        public ICommand NavigateCommand { get; set; }

        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            LoadedCommand = ReactiveCommand.CreateFromTask(InitAsync);
            NavigateCommand = ReactiveCommand.Create<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("NavRegion", navigatePath);
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
            ChatListManager.AddChat(list.ToArray());

            await LoadAllContact();
        }

        /// <summary>
        /// 加载所有通讯录
        /// </summary>
        /// <returns></returns>
        private async Task LoadAllContact()
        {
            var groupings = await Task.Run(() =>
            {
                //取到通讯录，过滤公众号，然后分组
                JObject contact_result = wcs.GetContact();
                List<WeChatUser> contact_all = contact_result["MemberList"]
                .Select(contact => JObjectToUser(contact))
                .Where(p=>p.StartChar!= "公众号")
                .OrderBy(p => p.StartChar).ToList();
                return contact_all.GroupBy(p => p.StartChar).OrderBy(p => p.Key).ToArray();
            });

            //将数据传输到通讯录组件
            ContactListManager.AddContact(groupings);
        }

        private WeChatUser JObjectToUser(JToken jObject)
        {
            WeChatUser user = jObject.ToObject<WeChatUser>();

            user.HeadImgUrl = wcs.GetIconUrl(user.UserName);
            user.ChatNotifyClose = user.IsChatNotifyClose();
            user.StartChar = user.GetStartChar();

            return user;
        }
    }
}
