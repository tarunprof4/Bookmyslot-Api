using Bookmyslot.BackgroundTasks.Api.Business;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Constants;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository;
using Bookmyslot.BackgroundTasks.Api.Repositories;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Bookmyslot.BackgroundTasks.Api.Injections
{
    public class BackgroundInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            LoadNotificationInjections(services);
            LoadCustomerInjections(services);
        }

        private static void LoadNotificationInjections(IServiceCollection services)
        {
            services.AddTransient<INotificationBusiness, NotificationBusiness>();
        }

        private static void LoadCustomerInjections(IServiceCollection services)
        {
            services.AddTransient<ICustomerBusiness, CustomerBusiness>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();


            var uri = new Uri("http://localhost:9200");
            var connectionPool = new SingleNodeConnectionPool(uri);

            var customerIndex = ElasticSearchConstants.CustomerIndex;
            var settings = new ConnectionSettings()
                           .DefaultMappingFor<CustomerModel>(m => m
                           .IndexName(customerIndex).IdProperty(p => p.Id))
                           .EnableHttpCompression();

            var elasticClient = new ElasticClient(settings);
            services.AddSingleton(elasticClient);

            var searchAsYouType = ElasticSearchConstants.SearchAsYouTypeField;
            if (!elasticClient.Indices.Exists(customerIndex).Exists)
            {
                var createIndexResponse = elasticClient.Indices.Create(customerIndex, c => c
       .Map<CustomerModel>(mm => mm
       .AutoMap<CustomerModel>()

       .Properties(p => p
       .Text(t => t.Name(n => n.FirstName)
       .Fields(ff => ff.SearchAsYouType(v => v.Name(searchAsYouType)))))

       .Properties(p => p
       .Text(t => t.Name(n => n.LastName)
       .Fields(ff => ff.SearchAsYouType(v => v.Name(searchAsYouType)))))

       .Properties(p => p
       .Text(t => t.Name(n => n.FullName)
       .Fields(ff => ff.SearchAsYouType(v => v.Name(searchAsYouType)))))

       ));

            }









        }




    }
}
