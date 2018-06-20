using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Webapi.Core.Template
{
    public static class AutofacConfig
    {
        public static IContainer AddAutofacContainer(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var assembly = Assembly.GetExecutingAssembly();
            //load all dependencies via Module. A Module class is added in all core folders
            builder.RegisterAssemblyModules(assembly);
            return builder.Build();
        }
    }
}
