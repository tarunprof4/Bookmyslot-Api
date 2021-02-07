using Bookmyslot.Api.Cache.Business;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Cache.Repositories;
using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Bookmyslot.Api.Injections
{
    public class CacheInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            CacheBusinessInjections(services);
            CacheRepositoryInjections(services);
        }


        private static void CacheBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<ITableCacheHandler, SqlTableCacheHandler>();
        }


        private static void CacheRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<ICacheRepository, CacheRepository>();

        }
    }
}
