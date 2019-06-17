using Prism.Regions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
    public class ContactListViewModel : ReactiveObject, IContactListManager, INavigationAware
    {
        private readonly List<WeChatUser> _allContactList = new List<WeChatUser>();

        public ObservableCollection<WeChatUser> ContactList { get; private set; } = new ObservableCollection<WeChatUser>();

        [Reactive]
        public WeChatUser SelectedItem { get; set; }

        [Dependency]
        protected IImageDownloadService ImageDownloadService { get; set; }

        public ContactListViewModel(IContactContentManager contactContentManager)
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(ContactList);
            cv.GroupDescriptions.Add(new PropertyGroupDescription(nameof(WeChatUser.StartChar)));

            this.WhenAnyValue(p => p.SelectedItem).Subscribe(p => contactContentManager.SelectedContact = p);
        }

        public void AddContact(params WeChatUser[] chat)
        {
            _allContactList.AddRange(chat);
            ContactList.AddRange(chat.Where(p => p.StartChar != "公众号"));
            ImageDownloadService.Add(ContactList.ToArray());
        }

        public WeChatUser FindContact(string userName)
        {
            return _allContactList.FirstOrDefault(p => p.UserName == userName);
        }

        public void DelContact(params string[] userNames)
        {
            foreach (var item in userNames)
            {
                var user = _allContactList.FirstOrDefault(p => p.UserName == item);
                if(user != null)
                {
                    _allContactList.Remove(user);
                    if (ContactList.Contains(user))
                        ContactList.Remove(user);
                }                
            }           
        }

        public void ModContact(params WeChatUser[] contact)
        {
            foreach (var item in contact)  
            {
                if (item.IsRoomContact())  //这里不包含群聊
                    continue;

                var local = _allContactList.FirstOrDefault(p => p.UserName == item.UserName);
                if (local != null)
                {
                    _allContactList.Remove(local);
                    ContactList.Remove(local);
                }
                _allContactList.Add(item);
                if (item.StartChar != "公众号")
                    ContactList.Add(item);
            }

            ImageDownloadService.Add(contact.ToArray());
        }

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            navigationContext.NavigationService.Region.RegionManager.RequestNavigate(WeChatClientConst.ContentRegionName, "ContactContentView");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion
    }
}
