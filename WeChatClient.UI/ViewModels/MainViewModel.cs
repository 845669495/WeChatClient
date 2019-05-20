using Newtonsoft.Json.Linq;
using Prism.Ioc;
using Prism.Modularity;
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
using WeChatClient.Core.Http;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.UI.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private WeChatService wcs = new WeChatService();

        private readonly IModuleManager _moduleManager;
        private readonly IContainerExtension _container;
        private readonly IChatListManager _chatListManager;

        [Reactive]
        public WeChatUser WeChatUser { get; private set; }

        public ICommand LoadedCommand { get; }

        public MainViewModel(IModuleManager moduleManager, IContainerExtension container)
        {
            _moduleManager = moduleManager;
            _container = container;

            moduleManager.LoadModule("ChatListModule");
            _chatListManager = _container.Resolve<IChatListManager>();

            LoadedCommand = ReactiveCommand.CreateFromTask(InitAsync);
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
                return init_result["ContactList"].Select(contact=> JObjectToUser(contact));
            });
            //将数据传输到联系人组件
            _chatListManager.AddChatUser(list.ToArray());

            await LoadAllContact();
        }

        /// <summary>
        /// 加载所有通讯录
        /// </summary>
        /// <returns></returns>
        private async Task LoadAllContact()
        {
            List<IGrouping<string, WeChatUser>> groupings = await Task.Run(() =>
            {
                //取到通讯录，过滤公众号，然后分组
                JObject contact_result = wcs.GetContact();
                List<WeChatUser> contact_all = contact_result["MemberList"]
                .Select(contact => JObjectToUser(contact))
                .Where(p=>p.StartChar!= "公众号")
                .OrderBy(p => p.StartChar).ToList();
                return contact_all.GroupBy(p => p.StartChar).OrderBy(p => p.Key).ToList();
            });

            //将数据传输到通讯录组件
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
