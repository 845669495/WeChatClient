using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Models;

namespace WeChatClient.Core.Interfaces
{
    public interface IChatListManager
    {
        void AddChatUser(params WeChatUser[] chat);
    }
}
