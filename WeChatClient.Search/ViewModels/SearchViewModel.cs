using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Unity.Attributes;
using WeChatClient.Core.Events;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.Search.ViewModels
{
    public class SearchViewModel : ReactiveObject
    {
        [Dependency]
        protected IContactListManager ContactListManager { get; set; }
        [Dependency]
        protected IChatListManager ChatListManager { get; set; }

        [Reactive]
        public string SearchText { get; set; }
        [Reactive]
        public bool IsOpen { get; set; }

        public ObservableCollection<WeChatUser> ContactList { get; private set; } = new ObservableCollection<WeChatUser>();

        public ICommand SelectCommand { get; }

        public SearchViewModel(IEventAggregator ea)
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(ContactList);
            cv.GroupDescriptions.Add(new PropertyGroupDescription(nameof(WeChatUser.SearchGroupDesc)));

            SelectCommand = ReactiveCommand.Create<WeChatUser>(c =>
            {
                ea.GetEvent<ChatWithContactEvent>().Publish(c);
                IsOpen = false;
                SearchText = null;
            });

            this.WhenAnyValue(p => p.SearchText).Select(p => !string.IsNullOrWhiteSpace(p)).Subscribe(p => IsOpen = p);
            this.WhenAnyValue(p => p.SearchText).Throttle(TimeSpan.FromMilliseconds(100))  //延迟100毫秒再异步查询
                .Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p?.Trim())
                .DistinctUntilChanged().Select(Search).ObserveOn(RxApp.MainThreadScheduler).Subscribe(UpdateData);
        }

        /// <summary>
        /// 查询好友和群聊
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private List<WeChatUser> Search(string searchText)
        {
            List<WeChatUser> list = new List<WeChatUser>();
            list.AddRange(ContactListManager.SearchContact(searchText));
            list.AddRange(ChatListManager.SearchRoomChat(searchText));
            return list;
        }

        private void UpdateData(List<WeChatUser> list)
        {
            ContactList.Clear();
            ContactList.AddRange(list);
        }
    }
}
