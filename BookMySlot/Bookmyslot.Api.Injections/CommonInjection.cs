using Bookmyslot.SharedKernel.Compression;
using Bookmyslot.SharedKernel.Contracts.Compression;
using Bookmyslot.SharedKernel.Contracts.Email;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.Contracts.Event;
using Bookmyslot.SharedKernel.Contracts.Logging;
using Bookmyslot.SharedKernel.Email;
using Bookmyslot.SharedKernel.Encryption;
using Bookmyslot.SharedKernel.Event;
using Bookmyslot.SharedKernel.Logging;
using Bookmyslot.SharedKernel.Logging.Enrichers;
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
            services.AddTransient<IEventDispatcher, EventDispatcher>();
            services.AddSingleton<ICompression, GZipCompression>();
            services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>();
            services.AddSingleton<ISymmetryEncryption, FlexibleAesSymmetricEncryption>();
            services.AddSingleton<IHashing, FlexibleSha256SaltedHash>();
            services.AddSingleton<ILoggerService, LoggerService>();

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
