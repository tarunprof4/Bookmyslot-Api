using Bookmyslot.Api.NodaTime.Business;
using Bookmyslot.Api.NodaTime.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class NodaTimeInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            LocationBusinessInjections(services);
        }
        private static void LocationBusinessInjections(IServiceCollection services)
        {
            services.AddSingleton<INodaTimeZoneLocationBusiness, TzdbNodaTimeZoneLocationBusiness>();
        }
    }
}
