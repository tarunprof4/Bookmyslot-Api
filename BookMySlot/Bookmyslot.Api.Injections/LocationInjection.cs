using Bookmyslot.Api.Authentication;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Authentication.Facebook;
using Bookmyslot.Api.Authentication.Google;
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
