using Bookmyslot.Api.Cache.Business;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Bookmyslot.Api.Injections
{
    public class CacheInjection
    {
        public static void LoadInjections(IServiceCollection services, Dictionary<string, string> appConfigurations)
        {
            CacheBusinessInjections(services);
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = appConfigurations[AppSettingKeysConstants.CacheDatabase];
            //    options.SchemaName = "dbo";
            //    options.TableName = TableNameConstants.Cache;
            //});

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379,DefaultDatabase=1";
            });
        }


        private static void CacheBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<IDistributedDatabaseCacheBuisness, DistributedDatabaseCacheBuisness>();
            services.AddTransient<IDistributedInMemoryCacheBuisness, DistributedInMemoryCacheBuisness>();
        }


     
    }
}
