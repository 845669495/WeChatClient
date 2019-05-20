using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WeChatClient.Login.ViewModels;

namespace WeChatClient.Login.Views
{
    /// <summary>
    /// HeadView.xaml 的交互逻辑
    /// </summary>
    public partial class HeadView : UserControl
    {
        public HeadView(HeadViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
