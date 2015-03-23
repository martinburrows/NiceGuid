using ConfigMapping;
using NiceGuid.Generator;
using NiceGuid.Web.Configuration;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NiceGuid.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NiceGuid.Web.App_Start.NinjectWebCommon), "Stop")]

namespace NiceGuid.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IGuidGeneratorFactory>().ToMethod(context => new GuidGeneratorFactory(HttpContext.Current.Server.MapPath("~/words.txt")));
            kernel.Bind<IAppSettings>().ToMethod(context => ConfigMapper.Map<IAppSettings>());
        }        
    }
}
