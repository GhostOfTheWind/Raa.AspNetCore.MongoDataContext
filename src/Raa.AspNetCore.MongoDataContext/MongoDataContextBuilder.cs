using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Raa.AspNetCore.MongoDataContext.Repository;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext
{
    public class MongoDataContextBuilder
    {
        public IServiceCollection Services { get; private set; }
        public Type ContextType { get; private set; }

        public MongoDataContextBuilder(Type context, IServiceCollection serviceCollection)
        {
            ContextType = context;

            Services = serviceCollection;
        }

        private MongoDataContextBuilder Add(Type serviceType)
        {
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Scoped);
            Services.Add(serviceDescriptor);

            return this;
        }

        public MongoDataContextBuilder AddRepository<TRepository>()
            where TRepository : class
        {
            Type baseType = typeof(Repository<>);
            Type repoType = typeof(TRepository);

            var isTypeRepository = isBaseType(repoType, baseType);
            if (!isTypeRepository) throw new ArgumentException(typeof(TRepository).Name);


            return Add(repoType);
        }

        public MongoDataContextBuilder CreateRepository<TEntity>()
            where TEntity : IEntity<ObjectId>
        {
            Type repoType = typeof(Repository<>).MakeGenericType(typeof(TEntity));

            return Add(repoType);
        }



        private bool isBaseType(Type currentType, Type baseType)
        {
            var type = currentType;
            while (type != null)
            {
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                

                if(genericType != null && genericType == baseType)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

    }
}
