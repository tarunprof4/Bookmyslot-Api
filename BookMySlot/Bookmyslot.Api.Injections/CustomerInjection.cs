using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Customers.Business;
using Bookmyslot.Api.Customers.Business.Validations;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories;
using Bookmyslot.Api.Customers.ViewModels;
using Bookmyslot.Api.Customers.ViewModels.Validations;
using Bookmyslot.Api.NodaTime.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class CustomerInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            CustomerViewModelsInjections(services);
            CustomerBusinessInjections(services);
            CustomerRepositoryInjections(services);
        }

        private static void CustomerViewModelsInjections(IServiceCollection services)
        {
            services.AddScoped<IValidator<AdditionalProfileSettingsViewModel>>(x => new AdditionalProfileSettingsViewModelValidator());
            services.AddScoped<IValidator<CustomerSettingsViewModel>>(x => new CustomerSettingsViewModelValidator(x.GetRequiredService<INodaTimeZoneLocationBusiness>()));
            services.AddScoped<IValidator<ProfileSettingsViewModel>>(x => new ProfileSettingsViewModelValidator());
            services.AddScoped<IValidator<RegisterCustomerViewModel>>(x => new RegisterCustomerViewModelValidator());
        }

        private static void CustomerBusinessInjections(IServiceCollection services)
        {
            services.AddScoped<IValidator<SocialCustomerLoginModel>>(x => new SocialLoginCustomerValidator());
            services.AddTransient<IRegisterCustomerBusiness, RegisterCustomerBusiness>();
            services.AddTransient<ILoginCustomerBusiness, LoginCustomerBusiness>();
            services.AddTransient<IProfileSettingsBusiness, ProfileSettingsBusiness>();
            services.AddTransient<IAdditionalProfileSettingsBusiness, AdditionalProfileSettingsBusiness>();
            services.AddTransient<ICustomerBusiness, CustomerBusiness>();
            services.AddTransient<ICustomerSettingsBusiness, CustomerSettingsBusiness>();
        }


        private static void CustomerRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<IRegisterCustomerRepository, RegisterCustomerRepository>();
            services.AddTransient<IProfileSettingsRepository, ProfileSettingsRepository>();
            services.AddTransient<IAdditionalProfileSettingsRepository, AdditionalProfileSettingsRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerSettingsRepository, CustomerSettingsRepository>();
        }
    }
}
