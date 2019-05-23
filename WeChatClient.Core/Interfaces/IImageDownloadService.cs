using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatClient.Core.Interfaces
{
    /// <summary>
    /// 图片下载服务
    /// </summary>
    public interface IImageDownloadService
    {
        /// <summary>
        /// 添加需要下载图片的模型
        /// </summary>
        /// <param name="models"></param>
        void Add(params INeedDownloadImageModel[] models);
    }
}
