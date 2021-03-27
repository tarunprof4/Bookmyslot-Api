using Bookmyslot.Api.Search.Business;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Search.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class SearchInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            SearchBusinessInjections(services);
            SearchRepositoryInjections(services);
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
