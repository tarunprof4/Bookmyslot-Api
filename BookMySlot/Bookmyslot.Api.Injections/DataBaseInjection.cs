﻿using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.SharedKernel.Contracts.Database;
using Bookmyslot.SharedKernel.Database;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;

namespace Bookmyslot.Api.Injections
{
    public class DataBaseInjection
    {
        public static void LoadInjections(IServiceCollection services, AppConfiguration appConfiguration)
        {
            DatabaseInjections(services, appConfiguration);
        }
        private static void DatabaseInjections(IServiceCollection services, AppConfiguration appConfiguration)
        {
            services.AddSingleton<IDbInterceptor, DbInterceptor>();
            services.AddTransient<IDbConnection>((sp) => new MySqlConnection(appConfiguration.DatabaseConnectionString));
        }
    }
}
