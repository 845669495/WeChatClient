using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core;
using WeChatClient.Core.Dependency;
using WeChatClient.Main.Views;

namespace WeChatClient.Main
{
    [Module(OnDemand = true)]  //按需加载模块
    [ModuleDependency(WeChatClientConst.ChatListModuleName)]   //设置模块依赖
    public class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(WeChatClientConst.MainRegionName, typeof(MainView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
