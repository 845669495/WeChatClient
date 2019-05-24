using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WeChatClient.Core.Events;
using WeChatClient.Core.Http;
using WeChatClient.Login.Events;
using WeChatClient.Login.Views;

namespace WeChatClient.Login
{
    public class LoginModule : IModule
    {      
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            var ea = containerProvider.Resolve<IEventAggregator>();

            IRegion region = regionManager.Regions[WeChatClientConst.MainRegionName];

            var qrView = containerProvider.Resolve<QRCodeView>();
            region.Add(qrView);

            var headView = containerProvider.Resolve<HeadView>();
            region.Add(headView);

            Task.Run(() =>
            {
                LoopLoginCheck(ea, region, qrView, headView);
            });

            ea.GetEvent<SwitchAccountEvent>().Subscribe(() =>
            {
                region.Activate(qrView);
            });
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        /// <summary>
        /// 循环检测是否登录了
        /// </summary>
        private void LoopLoginCheck(IEventAggregator ea, IRegion region, QRCodeView qrView, HeadView headView)
        {
            LoginService ls = new LoginService();

            object login_result = null;
            //循环判断手机扫描二维码结果
            while (true)
            {
                login_result = ls.LoginCheck();
                //已扫描 未登录
                if (login_result is ImageSource)
                {
                    ea.GetEvent<UpdateHeadImageEvent>().Publish(login_result as ImageSource);
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        region.Activate(headView);
                    })); 
                }
                //已完成登录
                if (login_result is string)
                {
                    //访问登录跳转URL
                    ls.GetSidUid(login_result as string);

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        region.RemoveAll();
                        ea.GetEvent<LoginSuccessfulEvent>().Publish();
                    }));                    
                    break;
                }
                ////超时
                if (login_result is int)
                {
                    ea.GetEvent<UpdateQRCodeEvent>().Publish();
                }
            }
        }
    }
}
