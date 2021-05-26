using Bookmyslot.Api.Common.Compression;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Compression;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Email;
using Bookmyslot.Api.Common.Encryption;
using Bookmyslot.Api.Common.Logging;
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
            services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>();
            services.AddSingleton<ISymmetryEncryption, AesSymmetricEncryption>();
            services.AddSingleton<IHashing, Sha256SaltedHash>();
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
