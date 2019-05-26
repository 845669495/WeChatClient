using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatClient.Core.Models
{
    /// <summary>
    /// 群成员
    /// </summary>
    public class ChatRoomMember
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 备注名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string ShowName
        {
            get
            {
                return string.IsNullOrEmpty(DisplayName) ? NickName : DisplayName;
            }
        }
        /// <summary>
        /// 关联的联系人对象
        /// </summary>
        public WeChatUser User { get; set; }
    }
}
