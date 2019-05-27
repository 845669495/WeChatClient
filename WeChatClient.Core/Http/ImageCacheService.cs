using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WeChatClient.Core.Dependency;

namespace WeChatClient.Core.Http
{
    /// <summary>
    /// 图片全局缓存
    /// </summary>
    [ExposeServices(ServiceLifetime.Singleton)]
    public class ImageCacheService
    {
        private readonly Dictionary<string, ImageSource> _cache = new Dictionary<string, ImageSource>();
        public bool TryGetValue(string uri, out ImageSource image)
        {
            try
            {
                return _cache.TryGetValue(uri, out image);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Add(string uri, ImageSource image)
        {
            lock (this)
            {
                if (_cache.ContainsKey(uri))
                    return;
                _cache.Add(uri, image);
            }
        }
    }
}
