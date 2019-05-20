using Newtonsoft.Json.Linq;
using Prism.Ioc;
using Prism.Modularity;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public MainViewModel(IModuleManager moduleManager, IContainerExtension container)
        {
            _moduleManager = moduleManager;
            _container = container;

            moduleManager.LoadModule("ChatListModule");
            _chatListManager = _container.Resolve<IChatListManager>();

            Init();
        }

        private void Init()
        {
            JObject init_result = wcs.WeChatInit();

            List<object> contact_all = new List<object>();
            if (init_result != null)
            {
                var me = new WeChatUser();
                me.UserName = init_result["User"]["UserName"].ToString();
                me.City = "";
                me.HeadImgUrl = wcs.GetIconUrl(me.UserName);
                me.NickName = init_result["User"]["NickName"].ToString();
                me.Province = "";
                me.PyQuanPin = init_result["User"]["PYQuanPin"].ToString();
                me.RemarkName = init_result["User"]["RemarkName"].ToString();
                me.RemarkPYQuanPin = init_result["User"]["RemarkPYQuanPin"].ToString();
                me.Sex = init_result["User"]["Sex"].ToString();
                me.Signature = init_result["User"]["Signature"].ToString();
                WeChatUser = me;

                var chatList = new List<WeChatUser>();
                //部分好友名单
                foreach (JObject contact in init_result["ContactList"])
                {
                    WeChatUser user = new WeChatUser();
                    user.UserName = contact["UserName"].ToString();
                    user.City = contact["City"].ToString();
                    user.HeadImgUrl = wcs.GetIconUrl(user.UserName);
                    user.NickName = contact["NickName"].ToString();
                    user.Province = contact["Province"].ToString();
                    user.PyQuanPin = contact["PYQuanPin"].ToString();
                    user.RemarkName = contact["RemarkName"].ToString();
                    user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                    user.Sex = contact["Sex"].ToString();
                    user.Signature = contact["Signature"].ToString();
                    user.SnsFlag = contact["SnsFlag"].ToString();
                    user.KeyWord = contact["KeyWord"].ToString();
                    chatList.Add(user);
                }

                _chatListManager.AddChatUser(chatList.ToArray());
            }
        }
    }
}
