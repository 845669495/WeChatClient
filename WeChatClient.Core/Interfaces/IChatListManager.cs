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
        /// <summary>
        /// 添加聊天
        /// </summary>
        /// <param name="chat"></param>
        void AddChat(params WeChatUser[] chat);
        /// <summary>
        /// 同步消息
        /// </summary>
        /// <param name="messages"></param>
        void SyncMessage(params WeChatMessage[] messages);
    }
}
