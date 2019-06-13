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
        /// 修改聊天
        /// </summary>
        /// <param name="chat"></param>
        void ModChat(params WeChatUser[] chat);
        /// <summary>
        /// 删除聊天
        /// </summary>
        /// <param name="userNames"></param>
        void DelChat(params string[] userNames);
        /// <summary>
        /// 是否包含该用户名的聊天
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool Contains(string userName);
        /// <summary>
        /// 同步消息
        /// </summary>
        /// <param name="messages"></param>
        void SyncMessage(params WeChatMessage[] messages);
        /// <summary>
        /// 更新初始化的群成员
        /// </summary>
        /// <param name="groupChats"></param>
        void UpdateInitGroupMember(params WeChatUser[] groupChats);
    }
}
