using Bookmyslot.Api.Common.Compression;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Database;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Common.Logging;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Bookmyslot.Api.Common.Logging.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Common.Injections
{
    public class CommonInjection
    {

        public static void CommonInjections(IServiceCollection services)
        {
            services.AddSingleton<ICompression, GZipCompression>();

            LoggingInjections(services);
            DatabaseInjections(services);
        }

        private static void LoggingInjections(IServiceCollection services)
        {
            services.AddTransient<ILoggerService, LoggerService>();
            services.AddTransient<IAppLogContext, AppLogContext>();
            services.AddTransient<DefaultLogEnricher>();
        }

        private static void DatabaseInjections(IServiceCollection services)
        {
            services.AddSingleton<IDbInterceptor, DbInterceptor>();
        }

    }
}
