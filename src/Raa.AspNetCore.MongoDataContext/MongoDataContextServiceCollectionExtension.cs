using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext
{
    public static class MongoDataContextServiceCollectionExtension
    {
        public static MongoDataContextBuilder AddMongoDataContext<TContext>(this IServiceCollection services, Action<MongoDataContextOptions> config)
            where TContext : MongoDataContext
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            services.Configure<MongoDataContextOptions>(config);

            services.TryAdd(new ServiceDescriptor(typeof(TContext), typeof(TContext), ServiceLifetime.Scoped));
            
            return new MongoDataContextBuilder(typeof(TContext), services);
        }

        
    }


}
