﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeChatClient.ChatContent.ViewModels;

namespace WeChatClient.ChatContent.Views
{
    /// <summary>
    /// ChatContentView.xaml 的交互逻辑
    /// </summary>
    public partial class ChatContentView : UserControl
    {
        public ChatContentView(ChatContentViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
