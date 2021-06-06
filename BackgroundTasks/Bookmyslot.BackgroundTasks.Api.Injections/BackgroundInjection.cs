using Bookmyslot.BackgroundTasks.Api.Business;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.BackgroundTasks.Api.Injections
{
    public class BackgroundInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            services.AddTransient<INotificationBusiness, NotificationBusiness>();
        }

     
    }
}
