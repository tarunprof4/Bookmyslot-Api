
using Bookmyslot.Api.Common.Contracts.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.BackgroundTasks.Api.Injections
{
    public class AppConfigurationInjection
    {
        public static void LoadInjections(IServiceCollection services, IConfiguration configuration)
        {
            AppInjections(services, configuration);
        }
        private static void AppInjections(IServiceCollection services, IConfiguration configuration)
        {
            var cacheConfiguration = new Contracts.Configuration.AppConfiguration(configuration);
            services.AddSingleton(cacheConfiguration);
            var emailConfiguration = new EmailConfiguration(configuration);
            services.AddSingleton(emailConfiguration);
        }
    }
}
