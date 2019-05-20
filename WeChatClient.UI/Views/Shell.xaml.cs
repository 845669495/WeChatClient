using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeChatClient.Core.Events;
using WeChatClient.UI.ViewModels;

namespace WeChatClient.UI.Views
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : Window
    {
        private readonly IContainerExtension _containerProvider;
        private readonly IEventAggregator _ea;
        public Shell(IContainerExtension containerProvider)
        {
            InitializeComponent();
            _containerProvider = containerProvider;
            _ea = containerProvider.Resolve<IEventAggregator>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ea.GetEvent<LoginSuccessfulEvent>().Subscribe(() =>
            {
                var regionManager = _containerProvider.Resolve<IRegionManager>();
                regionManager.RegisterViewWithRegion("MainRegion", typeof(MainView));
                SizeToContent = SizeToContent.Manual;
                Width = 1000;
                Height = 700;
            }, ThreadOption.UIThread);
        }
    }
}
