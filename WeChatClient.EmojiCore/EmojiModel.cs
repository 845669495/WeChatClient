using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WeChatClient.EmojiCore
{
    /// <summary>
    /// 表情数据模型
    /// </summary>
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

        public override string ToString()
        {
            if (Type == EmojiType.Emoji)
                return $"<span class=\"emoji emoji{Code}\"></span>";
            else
                return $"[{Code}]";
        }
    }

    /// <summary>
    /// 表情类型
    /// </summary>
    public enum EmojiType
    {
        /// <summary>
        /// Emoji表情
        /// </summary>
        Emoji,
        /// <summary>
        /// QQ表情
        /// </summary>
        QQ
    }

    /// <summary>
    /// 表情大小
    /// </summary>
    public enum EmojiSize
    {
        /// <summary>
        /// 小的是富文本中的表情
        /// </summary>
        Small,
        /// <summary>
        /// 中的是表情对话框中的表情
        /// </summary>
        Medium
    }
}
