using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Azure.Repositories;
using Bookmyslot.Api.Common.Contracts.Event.Interfaces;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Common.Event;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class AzureInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            services.AddTransient<IEventDispatcher, EventDispatcher>();
            AzureEventGridInjections(services);
            AzureRepositoryInjections(services);
        }


        private static void AzureEventGridInjections(IServiceCollection services)
        {
            services.AddTransient<IEventGridService, EventGridService>();
        }
        private static void AzureRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<IBlobRepository, BlobRepository>();
        }
    }
}
