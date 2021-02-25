using Bookmyslot.Api.Common.Contracts.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class AppConfigurationInjection
    {
        public static void LoadInjections(IServiceCollection services, IConfiguration configuration)
        {
            AppInjections(services, configuration);
        }
        private static void AppInjections(IServiceCollection services, IConfiguration configuration)
        {
            var cacheConfiguration = new CacheConfiguration(configuration);
            services.AddSingleton(cacheConfiguration);
            var emailConfiguration = new EmailConfiguration(configuration);
            services.AddSingleton(emailConfiguration);
            var appConfiguration = new AppConfiguration(configuration);
            services.AddSingleton(appConfiguration);

        }
    }
}
