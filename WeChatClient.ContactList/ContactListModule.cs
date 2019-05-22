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
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            containerRegistry.RegisterForNavigation<ContactListView>();  //将联系人列表注册到导航
        }
    }
}
