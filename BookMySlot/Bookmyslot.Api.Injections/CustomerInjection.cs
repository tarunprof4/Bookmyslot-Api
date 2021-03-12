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
            services.AddTransient<IRegisterCustomerBusiness, RegisterCustomerBusiness>();
            services.AddTransient<ILoginCustomerBusiness, LoginCustomerBusiness>();
            services.AddTransient<IProfileSettingsBusiness, ProfileSettingsBusiness>();
            services.AddTransient<ICustomerBusiness, CustomerBusiness>();
            services.AddTransient<ICustomerAdditionalInformationBusiness, CustomerAdditionalInformationBusiness>();
        }


        private static void CustomerRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<IRegisterCustomerRepository, RegisterCustomerRepository>();
            services.AddTransient<IProfileSettingsRepository, ProfileSettingsRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerAdditionalInformationRepository, CustomerAdditionalInformationRepository>();
        }
    }
}
