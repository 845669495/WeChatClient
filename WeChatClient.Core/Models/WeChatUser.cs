using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatClient.Core.Models
{
    public class WeChatUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像URL
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 备注名
        /// </summary>
        public string RemarkName { get; set; }
        /// <summary>
        /// 性别，0-未设置（公众号、保密），1-男，2-女
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 昵称全拼
        /// </summary>
        public string PyQuanPin { get; set; }
        /// <summary>
        /// 备注名全拼
        /// </summary>
        public string RemarkPYQuanPin { get; set; }
        /// <summary>
        /// 没发现关键性标注
        /// </summary>
        public string ContactFlag { get; set; }
        /// <summary>
        /// 0：是公众号或者是群聊 其他值是好友   //也有特殊情况，还是不行
        /// </summary>
        public string SnsFlag { get; set; }
        /// <summary>
        /// 公众号是gh_
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string ShowName
        {
            get
            {
                return string.IsNullOrEmpty(RemarkName) ? NickName : RemarkName;
            }
        }
        /// <summary>
        /// 显示的拼音全拼
        /// </summary>
        public string ShowPinYin
        {
            get
            {
                return string.IsNullOrEmpty(RemarkPYQuanPin) ? PyQuanPin : RemarkPYQuanPin;
            }
        }
       
        /// <summary>
        /// 分组的头
        /// </summary>
        public string StartChar { get; set; }

        /// <summary>
        /// 未读条数
        /// </summary>
        public int UnReadCount { get; set; }

        /// <summary>
        /// 最后消息时间
        /// </summary>
        public string LastTime { get; set; }
        
        /// <summary>
        /// 最后的消息
        /// </summary>
        public string LastMsg { get; set; }
    }
}
