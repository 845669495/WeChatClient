using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatClient.Core.Http;

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

        public static bool IsRoomContact(this WeChatUser user)
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

        /// <summary>
        /// 获取分组的头
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetStartChar(this WeChatUser user)
        {
            string start_char;

            if (user.KeyWord == "gh_" && user.SnsFlag.Equals("0") || user.KeyWord == "cmb")//user.KeyWord =="cmb"是招商银行信用卡，实在找不出区别了
            {
                start_char = "公众号";
            }
            else if (user.UserName.Contains("@@") && user.SnsFlag.Equals("0"))
            {
                start_char = "群聊";
            }
            else
            {
                start_char = string.IsNullOrEmpty(user.ShowPinYin) ? string.Empty : user.ShowPinYin[0].ToString().ToUpper();
            }
            return start_char;
        }

        /// <summary>
        /// 根据用户名获取图片地址
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetIconUrl(this string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            string _iconUrl;
            //讨论组
            if (userName.Contains("@@"))
            {
                _iconUrl = StaticUrl.stringWx + StaticUrl.Url_GetHeadImg + userName;
            }
            //好友
            else if (userName.Contains("@"))
            {
                _iconUrl = StaticUrl.stringWx + StaticUrl.Url_GetIcon + userName;
            }
            else
            {
                _iconUrl = StaticUrl.stringWx + StaticUrl.Url_GetIcon + userName;
            }
            return _iconUrl;
        }
    }
}
