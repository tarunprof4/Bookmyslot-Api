using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Database;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.File.Business;
using Bookmyslot.Api.File.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;

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
            services.AddTransient<IFileBusiness, FileBusiness>();
        }
    }
}
