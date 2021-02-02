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
            //var configuration = new ConfigurationBuilder()
            //                    .AddJsonFile("appsettings.json")
            //                    .Build();


            //string appVersion = configuration.GetSection(AppConfigurationConstants.AppVersion).Value;
            //string staticLogOutputTemplate = configuration.GetSection(AppConfigurationConstants.LogSettings).GetSection(AppConfigurationConstants.StaticLogOutPutTemplate).Value;
            //string elasticSearchUrl = configuration.GetSection(AppConfigurationConstants.ElasticSearchUrl).Value;


            //Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Verbose()
            //.Enrich.With(new StaticDefaultLogEnricher(appVersion))
            //.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
            // outputTemplate: staticLogOutputTemplate)
            //.CreateLogger();


            //Log.Logger = new LoggerConfiguration()
            //                       .MinimumLevel.Verbose()
            //                       .Enrich.With(new StaticDefaultLogEnricher(appVersion))
            //                       .Enrich.WithElasticApmCorrelationInfo()
            //                       .WriteTo.Async(a => a.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
            //                       {
            //                           CustomFormatter = new EcsTextFormatter()
            //                       }))
            //                       .CreateLogger();



            //        Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose()
            //                               .Enrich.With(new StaticDefaultLogEnricher(appVersion))
            //.WriteTo
            //.MSSqlServer(
            //    connectionString: "Data Source=.;Initial Catalog=LogDb;Integrated Security=True",
            //    sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents" })
            //.CreateLogger();


            try
            {
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
