using Bookmyslot.Api.Location.Business;
using Bookmyslot.Api.Location.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class LocationInjection
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
