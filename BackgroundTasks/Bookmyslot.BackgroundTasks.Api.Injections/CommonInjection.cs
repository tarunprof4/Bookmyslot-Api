using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Email;
using Bookmyslot.BackgroundTasks.Api.Logging;
using Bookmyslot.BackgroundTasks.Api.Logging.Enrichers;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.BackgroundTasks.Api.Injections
{
    public class CommonInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            LoggingInjections(services);
            EmailInjections(services);
        }

        private static void LoggingInjections(IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddTransient<DefaultLogEnricher>();
        }

        private static void EmailInjections(IServiceCollection services)
        {
            services.AddTransient<IEmailInteraction, EmailInteraction>();
            services.AddTransient<IEmailClient, EmailClient>();
        }

    }
}
