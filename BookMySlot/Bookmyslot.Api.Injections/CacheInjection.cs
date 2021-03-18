using Bookmyslot.Api.Cache.Business;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class CacheInjection
    {
        public static void LoadInjections(IServiceCollection services, AppConfiguration appConfiguration)
        {
            CacheBusinessInjections(services);
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = appConfiguration.CacheDatabaseConnectionString;
            //    options.SchemaName = "dbo";
            //    options.TableName = DatabaseConstants.CacheTable;
            //});

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = appConfiguration.CacheDatabaseConnectionString;
            });
        }


        private static void CacheBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<IDistributedDatabaseCacheBuisness, DistributedDatabaseCacheBusiness>();
            services.AddTransient<IDistributedInMemoryCacheBuisness, DistributedInMemoryCacheBusiness>();
        }


     
    }
}
