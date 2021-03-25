using Bookmyslot.Api.File.Business;
using Bookmyslot.Api.File.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class FileInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            FileBusinessInjections(services);
        }
        private static void FileBusinessInjections(IServiceCollection services)
        {
            services.AddSingleton<IFileConfigurationBusiness, FileConfigurationBusiness>();
        }
    }
}
