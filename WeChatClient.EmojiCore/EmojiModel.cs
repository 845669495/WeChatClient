using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WeChatClient.EmojiCore
{
    public class EmojiModel
    {
        /// <summary>
        /// 编码值
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 图片位置
        /// </summary>
        public Point Position { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public EmojiType Type { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public EmojiSize Size { get; set; }
    }

    public enum EmojiType
    {
        Emoji,
        QQ
    }
    public enum EmojiSize
    {
        Small,
        Medium
    }
}
