using Prism;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using WeChatClient.Core.Http;
using WeChatClient.Login.Events;

namespace WeChatClient.Login.ViewModels
{
    public class QRCodeViewModel : ReactiveObject
    {
        private LoginService ls = new LoginService();

        [Reactive]
        public ImageSource QRCode { get; private set; }

        public QRCodeViewModel(IEventAggregator ea)
        {
            SetQRCode();

            ea.GetEvent<UpdateQRCodeEvent>().Subscribe(SetQRCode, ThreadOption.UIThread);
        }

        private void SetQRCode()
        {
            QRCode = ls.GetQRCode();
        }
    }
}
