using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database;
using Bookmyslot.Api.Common.Database.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bookmyslot.Api.Injections
{
    public class DataBaseInjection
    {
        public static void LoadInjections(IServiceCollection services, Dictionary<string, string> appConfigurations)
        {
            DatabaseInjections(services, appConfigurations);
        }
        private static void DatabaseInjections(IServiceCollection services, Dictionary<string, string> appConfigurations)
        {
            services.AddSingleton<IDbInterceptor, DbInterceptor>();
            services.AddTransient<IDbConnection>((sp) => new SqlConnection(appConfigurations[AppConfigurationConstants.BookMySlotDatabaseConnectionString]));
        }
    }
}
