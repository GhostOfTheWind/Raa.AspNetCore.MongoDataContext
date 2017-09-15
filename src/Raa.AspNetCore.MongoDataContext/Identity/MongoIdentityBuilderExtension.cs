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

            InitStoreAndRepo(builder.UserType, Services, typeof(IUserStore<>), typeof(UserStore<>));
            InitStoreAndRepo(builder.RoleType, Services, typeof(IRoleStore<>), typeof(RoleStore<>));

            return builder;
        }

        private static void InitStoreAndRepo(Type type, IServiceCollection Services, Type service, Type implementation)
        {
            var repoType = typeof(Repository<>).MakeGenericType(type);

            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(repoType, repoType, ServiceLifetime.Scoped);
            Services.Add(serviceDescriptor);

            var genericServiceType = service.MakeGenericType(type);
            var genericImplementationType = implementation.MakeGenericType(type);
            Services.TryAddScoped(genericServiceType, genericImplementationType);
        }
    }




}
