using AdPang.FileManager.Common.Helper;
using AdPang.FileManager.Extensions.AOP;
using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.IServices.Base;
using AdPang.FileManager.Repositories.Base;
using AdPang.FileManager.Services.Base;
using AdPang.FileManager.Shared.Common.Helper;
using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Extensions.ServiceExtensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            var cacheType = new List<Type>();

            if (Appsettings.App(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjToBool ())
            {
                builder.RegisterType<TECCacheAOP>();
                cacheType.Add(typeof(TECCacheAOP));
            }

            if (Appsettings.App(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<TECLogAOP>();
                cacheType.Add(typeof(TECLogAOP));
            }


            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>)).InstancePerDependency();

            var assemblysServices = Assembly.Load("AdPang.FileManager.Services");//要记得!!!这个注入的是实现类层，不是接口层！不是 IServices
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy 对目标类型启用接口拦截。拦截器将被确定，通过在类或接口上截取属性, 或添加 InterceptedBy ()
                .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。;//指定已扫描程序集中的类型注册为提供所有其实现的接口。


            var assemblysRepository = Assembly.Load("AdPang.FileManager.Repositories");//模式是 Load(解决方案名)
            builder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces();

        }
    }
}
