using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.ChatContent.Views;
using WeChatClient.Core.Dependency;

namespace WeChatClient.ChatContent
{
    [Module(OnDemand = true)]
    public class ChatContentModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //内容区域默认显示聊天内容
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(WeChatClientConst.ContentRegionName, typeof(ChatContentView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
