using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class AppInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            AppInjections(services);
        }
        private static void AppInjections(IServiceCollection services)
        {
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
        }
    }
}
