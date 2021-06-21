using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Search.Business;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Search.Repositories;
using Bookmyslot.Bookmyslot.Api.Common.Search.Constants;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Bookmyslot.Api.Injections
{
    public class SearchInjection
    {
        public static void LoadInjections(IServiceCollection services, AppConfiguration appConfiguration)
        {
            ElasticSearchInjections(services, appConfiguration);
            SearchBusinessInjections(services);
            SearchRepositoryInjections(services);
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

        private static void SearchBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<ISearchCustomerBusiness, SearchCustomerBusiness>();
        }


        private static void SearchRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<ISearchRepository, SearchRepository>();
            services.AddTransient<ISearchCustomerRepository, SearchCustomerRepository>();
        }
    }
}
