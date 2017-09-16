using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Identity.MongoDb.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Identity.MongoDb
{
    public static class IdentityServiceCollectionExtensions
    {

        public static IdentityBuilder AddMongoDbStores<TContext>(this IdentityBuilder builder)
            where TContext : DbContext, new()
            
        {
            var userType = builder.UserType;
            var roleType = builder.RoleType;

            Type userStoreType = null;
            Type roleStoreType = null;

            userStoreType = typeof(UserStore<>).MakeGenericType(userType);
            roleStoreType = typeof(RoleStore<>).MakeGenericType(roleType);

            builder.Services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            builder.Services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);

            return builder;
        }

        public static IServiceCollection AddMongoDbContext<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.TryAdd(new ServiceDescriptor(typeof(TContext), typeof(TContext), ServiceLifetime.Scoped));

            return services;
        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
