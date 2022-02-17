using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
using UserManagement.Domain.Core.Interfaces;
using UserManagement.Domain.Core.Repository;
using UserManagement.Domain.Services;

namespace UserManagement.Domain
{
    public static class BootstrapperIntegrationExtensions
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IGenericContextFactory, DbContextFactory>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        //public static IUnityContainer RegisterDomainTypes(this IUnityContainer container)
        //{

        //    return container
        //            .RegisterType<IGenericContextFactory, DbContextFactory>(new TransientLifetimeManager())
        //            .RegisterType<IUnitOfWork, UnitOfWork>(new TransientLifetimeManager())
        //            .RegisterType<IUserService, UserService>(new TransientLifetimeManager())
        //        ;
        //}
    }
}
