using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WeChatClient.Login.Events;

namespace WeChatClient.Login.ViewModels
{
    public class ErrorViewModel : ReactiveObject
    {
        [Reactive]
        public string ErrorMsg { get; set; }

        public ErrorViewModel(IEventAggregator ea)
        {
            ea.GetEvent<ShowErrorMsgEvent>().Subscribe(i => ErrorMsg = i, ThreadOption.UIThread);
        }
    }
}
