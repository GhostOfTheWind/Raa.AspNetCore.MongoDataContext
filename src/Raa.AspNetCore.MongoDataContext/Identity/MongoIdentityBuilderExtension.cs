using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Raa.AspNetCore.MongoDataContext.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext.Identity
{
    public static class MongoIdentityBuilderExtension
    {
        public static IdentityBuilder AddMongoDataStores<TContext>(this IdentityBuilder builder)
            where TContext : MongoDataContext

        {
            var Services = builder.Services;


            InitRepo(Services, typeof(UserRepository<>), builder.UserType);
            InitRepo(Services, typeof(RoleRepository<>), builder.RoleType);

            InitStore(builder.UserType, Services, typeof(IUserStore<>), typeof(UserStore<>));
            InitStore(builder.RoleType, Services, typeof(IRoleStore<>), typeof(RoleStore<>));

            return builder;
        }

        private static void InitStore(Type type, IServiceCollection Services, Type service, Type implementation)
        {
            var repoType = typeof(Repository<>).MakeGenericType(type);

            

            var genericServiceType = service.MakeGenericType(type);
            var genericImplementationType = implementation.MakeGenericType(type);
            Services.TryAddScoped(genericServiceType, genericImplementationType);
        }

        private static void InitRepo(IServiceCollection Services, Type repoType, Type eneityType)
        {
            var genericRepoType = repoType.MakeGenericType(eneityType);

            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(genericRepoType, genericRepoType, ServiceLifetime.Scoped);
            Services.Add(serviceDescriptor);
        }
    }




}
