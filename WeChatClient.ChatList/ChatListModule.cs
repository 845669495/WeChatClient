using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.ChatList.Views;
using WeChatClient.Core.Dependency;

namespace WeChatClient.ChatList
{
    [Module(OnDemand = true)]  //按需加载模块
    public class ChatListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //导航区域默认显示聊天列表
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("NavRegion", typeof(ChatListView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
