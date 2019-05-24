using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WeChatClient.Core.Interfaces
{
    /// <summary>
    /// 需要下载图片的模型
    /// </summary>
    public interface INeedDownloadImageModel
    {
        /// <summary>
        /// 图片网络uri
        /// </summary>
        string Uri { get; }
        /// <summary>
        /// 图片对象
        /// </summary>
        ImageSource Image { set; get; }
    }
}
