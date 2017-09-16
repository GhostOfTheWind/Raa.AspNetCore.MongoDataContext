using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Raa.AspNetCore.MongoDataContext.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext
{
    public class MongoDataContextBuilder<TContext>
        where TContext : MongoDataContext
    {
        public IServiceCollection Services { get; private set; }

        public MongoDataContextBuilder(IServiceCollection serviceCollection)
        {
            Services = serviceCollection;
        }

        private MongoDataContextBuilder<TContext> Add(Type serviceType)
        {
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Scoped);
            Services.Add(serviceDescriptor);

            return this;
        }

        public MongoDataContextBuilder<TContext> AddRepository<TRepository>(string collectionName = null)
            where TRepository : class
        {
            Type baseType = typeof(Repository<>);
            Type repoType = typeof(TRepository);

            var isTypeRepository = isBaseType(repoType, baseType);
            if (!isTypeRepository) throw new ArgumentException(typeof(TRepository).Name);

            

            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(repoType, p => {
                var dataContext = p.GetRequiredService(typeof(TContext)) as TContext;

                var repoConstructors = repoType.GetConstructors().ToList();
                int paramsCount = 0;

                foreach (var constructor in repoConstructors)
                {
                    paramsCount = Math.Max(paramsCount, constructor.GetParameters().Length);
                }

                var instanceParams = new object[] { dataContext };

                if(paramsCount > 1)
                {
                    instanceParams = new object[] { dataContext, collectionName};
                }
                
                return Activator.CreateInstance(repoType, instanceParams); //todo: something to replace Activator
            }, ServiceLifetime.Scoped);
            
            Services.Add(serviceDescriptor);

            return this;
        }

        public MongoDataContextBuilder<TContext> CreateRepository<TEntity>(string collectionName = null)
            where TEntity : IEntity<ObjectId>
        {
            Type repoType = typeof(Repository<>).MakeGenericType(typeof(TEntity));
            
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(repoType, p => {
                var dataContext = p.GetRequiredService(typeof(TContext)) as TContext;

                return new Repository<TEntity>(dataContext, collectionName);
            }, ServiceLifetime.Scoped);

            Services.Add(serviceDescriptor);

            return this;
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
