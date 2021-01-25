using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
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
            string elasticSearchUrl = configuration.GetSection(AppConfigurationConstants.ElasticSearchUrl).Value;


            //Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Verbose()
            //.Enrich.WithProperty("Version", appVersion)
            //.Enrich.With(new StaticDefaultLogEnricher())
            //.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
            // outputTemplate: staticLogOutputTemplate)
            //.CreateLogger();


            Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
                                   .MinimumLevel.Verbose()
                                   .Enrich.With(new StaticDefaultLogEnricher())
                                   .Enrich.WithElasticApmCorrelationInfo()
                                   .WriteTo.Async(a => a.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
                                   {
                                       CustomFormatter = new EcsTextFormatter()
                                   }))
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
