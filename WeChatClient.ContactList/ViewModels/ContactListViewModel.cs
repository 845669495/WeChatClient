using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.ContactList.ViewModels
{
    [ExposeServices(ServiceLifetime.Singleton, typeof(IContactListManager))]  //注册为IContactListManager接口（单例）
    public class ContactListViewModel : ReactiveObject, IContactListManager
    {
        public ObservableCollection<IGrouping<string, WeChatUser>> ContactGroup { get; private set; } = new ObservableCollection<IGrouping<string, WeChatUser>>();

        public void AddContact(params IGrouping<string, WeChatUser>[] chat)
        {
            ContactGroup.AddRange(chat);
        }
    }
}
