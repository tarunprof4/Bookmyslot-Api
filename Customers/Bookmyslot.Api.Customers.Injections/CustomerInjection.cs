using Bookmyslot.Api.Customers.Business;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bookmyslot.Api.Customers.Injections
{
    public class CustomerInjection
    {
        public static void CustomerBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<ICustomerBusiness, CustomerBusiness>();
        }


        public static void CustomerRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        }
    }
}
