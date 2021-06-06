using Bookmyslot.Api.Common.Email.Configuration;
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
            var emailConfiguration = new EmailConfiguration(configuration);
            services.AddSingleton(emailConfiguration);
        }
    }
}
