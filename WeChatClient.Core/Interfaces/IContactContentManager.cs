using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Models;

namespace WeChatClient.Core.Interfaces
{
    public interface IContactContentManager
    {
        /// <summary>
        /// 当前选中的联系人
        /// </summary>
        WeChatUser SelectedContact { set; get; }
    }
}
