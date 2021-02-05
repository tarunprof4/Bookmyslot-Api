using Bookmyslot.Api.Customers.Business;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class CustomerInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            CustomerBusinessInjections(services);
            CustomerRepositoryInjections(services);
        }

        private static void CustomerBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<ICustomerBusiness, CustomerBusiness>();
        }


        private static void CustomerRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        }
    }
}
