using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Azure.Services.Event;
using Bookmyslot.Api.Azure.Services.Storage;
using Bookmyslot.SharedKernel.Contracts.EventGrid;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class AzureInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            AzureServicesInjections(services);
            AzureRepositoryInjections(services);
        }


        private static void AzureServicesInjections(IServiceCollection services)
        {
            services.AddTransient<IEventGridService, EventGridService>();
        }
        private static void AzureRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<IBlobRepository, BlobRepository>();
        }
    }
}
