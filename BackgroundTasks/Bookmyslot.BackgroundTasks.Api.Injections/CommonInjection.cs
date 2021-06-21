using Bookmyslot.Api.Common.Compression;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Compression;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Database;
using Bookmyslot.Api.Common.Email;
using Bookmyslot.BackgroundTasks.Api.Contracts.Configuration;
using Bookmyslot.BackgroundTasks.Api.Logging;
using Bookmyslot.BackgroundTasks.Api.Logging.Enrichers;
using Bookmyslot.Bookmyslot.Api.Common.Search.Constants;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Bookmyslot.BackgroundTasks.Api.Injections
{
    public class CommonInjection
    {
        public static void LoadInjections(IServiceCollection services, AppConfiguration appConfiguration)
        {
            CompressionInjections(services);
            LoggingInjections(services);
            EmailInjections(services);
            DatabaseInjections(services);
            ElasticSearchInjections(services, appConfiguration);
        }

        

        private static void CompressionInjections(IServiceCollection services)
        {
            services.AddSingleton<ICompression, GZipCompression>();
        }
        private static void LoggingInjections(IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddTransient<DefaultLogEnricher>();
        }

        private static void EmailInjections(IServiceCollection services)
        {
            services.AddTransient<IEmailInteraction, EmailInteraction>();
            services.AddTransient<IEmailClient, EmailClient>();
        }

        private static void DatabaseInjections(IServiceCollection services)
        {
            services.AddSingleton<IDbInterceptor, DbInterceptor>();
        }

        private static void ElasticSearchInjections(IServiceCollection services, AppConfiguration appConfiguration)
        {
            var uri = new Uri(appConfiguration.ElasticSearchUrl);
            var connectionPool = new SingleNodeConnectionPool(uri);

            var customerIndex = ElasticSearchConstants.CustomerIndex;
            var settings = new ConnectionSettings()
                           .DefaultMappingFor<SearchCustomerModel>(m => m
                           .IndexName(customerIndex).IdProperty(p => p.Id))
                           .EnableHttpCompression();

            var elasticClient = new ElasticClient(settings);
            services.AddSingleton(elasticClient);
        }

    }
}
