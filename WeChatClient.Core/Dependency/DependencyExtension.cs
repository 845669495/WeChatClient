using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeChatClient.Core.Dependency
{
    /// <summary>
    /// 依赖注入扩展类
    /// </summary>
    public static class DependencyExtension
    {
        /// <summary>
        /// 注册程序集中标注了ExposeServicesAttribute属性的类
        /// </summary>
        /// <param name="container"></param>
        /// <param name="assembly"></param>
        public static void RegisterAssembly(this IContainerRegistry container, Assembly assembly)
        {
            var types = assembly.GetTypes()
    .Where(
        type => type != null &&
                type.IsClass &&
                !type.IsAbstract &&
                !type.IsGenericType &&
                type.CustomAttributes.Any(t => t.AttributeType == typeof(ExposeServicesAttribute))
    ).ToArray();

            foreach (var type in types)
            {
                RegisterType(container, type);
            }            
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type"></param>
        private static void RegisterType(IContainerRegistry container, Type type)
        {
            foreach (var exposedServices in GetExposedServices(type))
            {
                foreach (var serviceType in exposedServices.ExposedServiceTypes)
                {
                    if (exposedServices.ServiceLifetime == ServiceLifetime.Singleton)
                    {
                        //Prism.Ioc使用Unity框架，默认注册为Transient，这里需要改变自身为Singleton
                        container.RegisterSingleton(type);  
                        container.RegisterSingleton(serviceType, type);
                    }
                    else if(exposedServices.ServiceLifetime == ServiceLifetime.Transient)
                    {
                        container.Register(serviceType, type);
                    }
                }

            }
        }

        /// <summary>
        /// 获取要注册的服务
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<ExposeServicesAttribute> GetExposedServices(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            var customExposedServices = typeInfo
                .GetCustomAttributes()
                .OfType<ExposeServicesAttribute>()
                .ToList();

            return customExposedServices;
        }
    }
}
