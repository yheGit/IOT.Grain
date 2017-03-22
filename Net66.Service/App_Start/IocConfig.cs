using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using Autofac.Integration.WebApi;
using Net66.Data;
using Net66.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Compilation;
using System.Web.Http;


namespace Net66.Service
{
    public class IocConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            builder.RegisterApiControllers(assemblys);

            builder.RegisterAssemblyTypes(assemblys).Where(p => p.Name.EndsWith("Core")).AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(GrainRepository<>)).As(typeof(IGrainRepository<>)).InstancePerLifetimeScope();

            var container = builder.Build();
            AutofacWebApiDependencyResolver dependencyResolver = new AutofacWebApiDependencyResolver(container);

            var configuration = GlobalConfiguration.Configuration;
            configuration.DependencyResolver=dependencyResolver;
        }
    }
}