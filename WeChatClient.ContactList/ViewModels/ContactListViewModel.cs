using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Attributes;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.ContactList.ViewModels
{
    [ExposeServices(ServiceLifetime.Singleton, typeof(IContactListManager))]  //注册为IContactListManager接口（单例）
    public class ContactListViewModel : ReactiveObject, IContactListManager
    {
        public ObservableCollection<IGrouping<string, WeChatUser>> ContactGroup { get; private set; } = new ObservableCollection<IGrouping<string, WeChatUser>>();

        [Dependency]
        protected IImageDownloadService ImageDownloadService { get; set; }

        public void AddContact(params IGrouping<string, WeChatUser>[] chatGroup)
        {
            ContactGroup.AddRange(chatGroup);
            ImageDownloadService.Add(chatGroup.SelectMany(p => p).ToArray());
        }
    }
}
