using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Events;
using WeChatClient.Core.Interfaces;
using WeChatClient.Core.Models;

namespace WeChatClient.ContactContent.ViewModels
{
    [ExposeServices(ServiceLifetime.Singleton, typeof(IContactContentManager))]
    public class ContactContentViewModel : ReactiveObject, IContactContentManager
    {
        /// <summary>
        /// 当前选中联系人
        /// </summary>
        [Reactive]
        public WeChatUser SelectedContact { get; set; }
        /// <summary>
        /// 有联系人被选中
        /// </summary>
        public bool HasContactSelected { [ObservableAsProperty]get; }

        public ICommand ToChatCommand { get; }

        public ContactContentViewModel(IEventAggregator ea)
        {
            var observable = this.WhenAnyValue(p => p.SelectedContact).Select(p => p != null);
            observable.ToPropertyEx(this, p => p.HasContactSelected);
            ToChatCommand = ReactiveCommand.Create(() =>
            {
                ea.GetEvent<ChatWithContactEvent>().Publish(SelectedContact);
            }, observable);
        }
    }
}
