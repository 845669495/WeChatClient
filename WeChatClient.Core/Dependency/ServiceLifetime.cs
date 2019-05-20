using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatClient.Core.Dependency
{
    /// <summary>
    /// 生命周期
    /// </summary>
    public enum ServiceLifetime
    {
        /// <summary>
        /// 单例
        /// </summary>
        Singleton = 0,
        /// <summary>
        /// 多实例
        /// </summary>
        Transient = 1
    }
}
