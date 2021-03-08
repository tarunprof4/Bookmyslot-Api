using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class HttpFactoryInjections
    {
        public static void LoadInjections(IServiceCollection services)
        {
            services.AddHttpClient();
        }
    }
}
