using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatClient.Core.Dependency
{
    /// <summary>
    /// 公开服务，用来注册接口
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExposeServicesAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; }

        public Type[] ExposedServiceTypes { get; }

        public ExposeServicesAttribute(ServiceLifetime serviceLifetime, params Type[] exposedServiceTypes)
        {
            ServiceLifetime = serviceLifetime;
            ExposedServiceTypes = exposedServiceTypes ?? new Type[0];
        }
    }
}
