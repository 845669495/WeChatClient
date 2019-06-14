using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Models;

namespace WeChatClient.Core.Interfaces
{
    public interface IChatContentManager
    {
        /// <summary>
        /// 设置当前选中的聊天
        /// </summary>
        WeChatUser SelectedChat { set; get; }
    }
}
