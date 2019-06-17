using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.ContactContent.Views;
using WeChatClient.Core.Dependency;

namespace WeChatClient.ContactContent
{
    [Module(OnDemand = true)]
    public class ContactContentModule: IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            containerRegistry.RegisterForNavigation<ContactContentView>();  //将联系人内容注册到导航
        }
    }
}
