using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatClient.Core.Models
{
    public static class WxStatus
    {
        /// <summary>
        /// 联系人消息免打扰
        /// </summary>
        private const int CONTACTFLAG_NOTIFYCLOSECONTACT = 512;
        /// <summary>
        /// 群聊消息免打扰
        /// </summary>
        private const int CHATROOM_NOTIFY_CLOSE = 0;

        private static bool IsRoomContact(WeChatUser user)
        {
            return user.UserName.StartsWith("@@");
        }

        /// <summary>
        /// 是否消息免打扰
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsChatNotifyClose(this WeChatUser user)
        {
            return user.UserName.StartsWith("@@") 
                ? user.Statues == CHATROOM_NOTIFY_CLOSE 
                : (user.ContactFlag & CONTACTFLAG_NOTIFYCLOSECONTACT) > 0;
        }
    }
}
