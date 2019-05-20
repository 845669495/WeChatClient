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
    public class HeadViewModel : ReactiveObject
    {
        [Reactive]
        public ImageSource HeadImage { get; set; }

        public ICommand SwitchAccountCommand { get; }

        public HeadViewModel(IEventAggregator ea)
        {
            SwitchAccountCommand = ReactiveCommand.Create(() =>
            {
                ea.GetEvent<SwitchAccountEvent>().Publish();
            });

            ea.GetEvent<UpdateHeadImageEvent>().Subscribe(i => HeadImage = i, ThreadOption.UIThread);
        }
    }
}
