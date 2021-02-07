using Bookmyslot.Api.Common.Compression;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Email;
using Bookmyslot.Api.Common.Email.Interfaces;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class CommonInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            CommonInjections(services);
            LoggingInjections(services);
            EmailInjections(services);
        }

        private static void CommonInjections(IServiceCollection services)
        {
            services.AddSingleton<ICompression, GZipCompression>();
            services.AddSingleton<IKeyEncryptor, KeyEncryptor>();

            EmailInjections(services);
            LoggingInjections(services);
        }

        private static void LoggingInjections(IServiceCollection services)
        {
            services.AddTransient<DefaultLogEnricher>();
        }

        private static void EmailInjections(IServiceCollection services)
        {
            services.AddTransient<IEmailInteraction, EmailInteraction>();
            services.AddTransient<IEmailClient, EmailClient>();
        }

    }
}
