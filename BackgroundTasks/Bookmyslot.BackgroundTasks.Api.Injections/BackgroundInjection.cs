using Bookmyslot.BackgroundTasks.Api.Business;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository;
using Bookmyslot.BackgroundTasks.Api.Repositories;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}
