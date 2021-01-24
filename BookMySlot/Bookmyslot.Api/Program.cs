using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Bookmyslot.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();


            string appVersion = configuration.GetSection(AppConfigurationConstants.AppVersion).Value;
            string staticLogOutputTemplate = configuration.GetSection(AppConfigurationConstants.LogSettings).GetSection(AppConfigurationConstants.StaticLogOutPutTemplate).Value;

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithProperty("Version", appVersion)
            .Enrich.With(new StaticDefaultLogEnricher())
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
             outputTemplate: staticLogOutputTemplate)
            .CreateLogger();

            try
            {
                Log.Debug("Starting Bookmyslot web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
