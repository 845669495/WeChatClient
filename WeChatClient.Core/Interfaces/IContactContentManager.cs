using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeChatClient.Core.Models;

namespace WeChatClient.Core.Interfaces
{
    public interface IContactContentManager
    {
        /// <summary>
        /// 当前选中的联系人
        /// </summary>
        WeChatUser SelectedContact { set; get; }
        /// <summary>
        /// 去和选中的联系人聊天
        /// </summary>
        ICommand ToChatCommand { get; }
    }
}
