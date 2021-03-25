using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Azure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class AzureInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            AzureRepositoryInjections(services);
        }

       
        private static void AzureRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<IBlobRepository, BlobRepository>();
        }
    }
}
