﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatClient.Core.Models
{
    /// <summary>
    /// 微信消息
    /// </summary>
    public class WeChatMessage
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
        /// 消息接收人
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public int MsgType { get; set; }
        /// <summary>
        /// 消息时间（时间戳格式）
        /// </summary>
        public int CreateTime { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string CreateShortTime { get; set; }

        /// <summary>
        /// 是否为收到的消息
        /// </summary>
        public bool IsReceive { get; set; }
    }
}
