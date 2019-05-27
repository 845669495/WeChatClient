using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Unity.Attributes;
using WeChatClient.Core.Dependency;
using WeChatClient.Core.Helpers;
using WeChatClient.Core.Interfaces;

namespace WeChatClient.Core.Http
{
    [ExposeServices(ServiceLifetime.Transient,typeof(IImageDownloadService))]  //因为多个模块都需要下载图片，为了互不影响，则采用多实例
    public class ImageDownloadService : IImageDownloadService
    {
        /// <summary>
        /// 需要下载图片的模型队列
        /// </summary>
        private readonly Queue<INeedDownloadImageModel> _modelQueue = new Queue<INeedDownloadImageModel>();
        private readonly object _sync = new object();

        [Dependency]
        protected ImageCacheService ImageCacheService { get; set; }
        public ImageDownloadService()
        {
            StartDownloadTask();
        }

        /// <summary>
        /// 添加模型
        /// </summary>
        /// <param name="models"></param>
        public void Add(params INeedDownloadImageModel[] models)
        {
            lock (_sync)
            {
                foreach (var item in models)
                {
                    _modelQueue.Enqueue(item);
                }
            }
        }

        /// <summary>
        /// 开启下载图片后台任务
        /// </summary>
        private void StartDownloadTask()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (_modelQueue.Count > 0)
                    {
                        INeedDownloadImageModel model = null;
                        lock (_sync)
                        {
                            model = _modelQueue.Dequeue();
                        }
                        try
                        {
                            if (model.Image != null)
                                continue;

                            ImageSource image;
                            if (ImageCacheService.TryGetValue(model.Uri,out image))
                            {
                                model.Image = image;
                                continue;
                            }
                            byte[] bytes = BaseService.Request(model.Uri, MethodEnum.GET);
                            //这里赋值ImageSource时，需要在UI线程上执行，才能绑定到界面
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                try
                                {
                                    model.Image = ImageHelper.MemoryToImageSourceOther(new MemoryStream(bytes));
                                    ImageCacheService.Add(model.Uri, model.Image);
                                }
                                catch (Exception)
                                {
                                    
                                }
                            }));
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            });
        }
    }
}
