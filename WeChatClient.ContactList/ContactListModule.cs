using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.ContactList.Views;
using WeChatClient.Core.Dependency;

namespace WeChatClient.ContactList
{
    [Module(OnDemand = true)]  //按需加载模块
    public class ContactListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //var regionManager = containerProvider.Resolve<IRegionManager>();
            //regionManager.RegisterViewWithRegion("NavRegion", typeof(ContactListView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            //containerRegistry.RegisterForNavigation<ContactListView>();
        }
    }
}
