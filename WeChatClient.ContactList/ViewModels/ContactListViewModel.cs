using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Unity.Attributes;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.ContactList.ViewModels
{
    [ExposeServices(ServiceLifetime.Singleton, typeof(IContactListManager))]  //注册为IContactListManager接口（单例）
    public class ContactListViewModel : ReactiveObject, IContactListManager
    {
        public ObservableCollection<WeChatUser> ContactList { get; private set; } = new ObservableCollection<WeChatUser>();

        [Dependency]
        protected IImageDownloadService ImageDownloadService { get; set; }

        public ContactListViewModel()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(ContactList);
            cv.GroupDescriptions.Add(new PropertyGroupDescription(nameof(WeChatUser.StartChar)));
        }

        public void AddContact(params WeChatUser[] chat)
        {
            ContactList.AddRange(chat);
            ImageDownloadService.Add(chat);
        }
    }
}
