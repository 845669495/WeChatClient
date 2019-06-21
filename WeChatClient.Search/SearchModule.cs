using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Dependency;
using WeChatClient.Search.Views;

namespace WeChatClient.Search
{
    [Module(OnDemand = true)]
    [ModuleDependency(WeChatClientConst.ChatListModuleName)]
    public class SearchModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(WeChatClientConst.SearchRegionName, typeof(SearchView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
