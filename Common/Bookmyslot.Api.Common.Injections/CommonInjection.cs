using Bookmyslot.Api.Common.Compression;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Common.Injections
{
    public class CommonInjection
    {

        public static void CommonInjections(IServiceCollection services)
        {
            services.AddSingleton<ICompression, GZipCompression>();
        }

    }
}
