using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Search.Business;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Search.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bookmyslot.Api.Search.Injections
{
    public class SearchInjection
    {
        public static void SearchBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<ISearchCustomerBusiness, SearchCustomerBusiness>();
        }


        public static void CustomerRepositoryInjections(IServiceCollection services, Dictionary<string, string> appConfigurations)
        {
            services.AddTransient<ISearchCustomerRepository, SearchCustomerRepository>();
            services.AddTransient<IDbConnection>((sp) => new SqlConnection(appConfigurations[AppConfigurationConstants.BookMySlotDatabaseConnectionString]));
        }
    }
}
