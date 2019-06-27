using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WeChatClient.Core.Interfaces;

namespace WeChatClient.Core.Models
{
    /// <summary>
    /// 微信消息
    /// </summary>
    public class WeChatMessage: BaseNotifyModel, INeedDownloadImageModel
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 消息发送人
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息发送人显示名称
        /// </summary>
        public string FromUserShowName { get; set; }
        /// <summary>
        /// 消息接收人
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息内容（富文本）
        /// </summary>
        public TextBlock TbContent { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int MsgType { get; set; }
        /// <summary>
        /// 消息时间（时间戳格式）
        /// </summary>
        public int CreateTime { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string ShortTime { get; set; }
        /// <summary>
        /// 是否显示时间
        /// </summary>
        public bool ShowShortTime { get; set; }

        /// <summary>
        /// 状态通知码（如果为4，则加载更多聊天列表）
        /// </summary>
        public int StatusNotifyCode { get; set; }
        /// <summary>
        /// 状态通知用户名（如果状态通知码为4，则加载这些用户，添加到聊天列表）
        /// </summary>
        public string StatusNotifyUserName { get; set; }

        /// <summary>
        /// 是否为加载更多聊天列表消息
        /// </summary>
        public bool IsLoadMoreChats => MsgType == 51 && StatusNotifyCode == 4 && FromUserName == ToUserName;

        /// <summary>
        /// 是否为收到的消息
        /// </summary>
        public bool IsReceive { get; set; }
        /// <summary>
        /// 是否为群消息
        /// </summary>
        public bool IsRoom { get; set; }
        /// <summary>
        /// 只有收到的群消息需要显示发消息的人的名字
        /// </summary>
        public bool NeedShowName => IsReceive && IsRoom;

        public string Uri { get; set; }

        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }
    }
}
