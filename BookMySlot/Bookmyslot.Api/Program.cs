using Bookmyslot.Api.Common.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Bookmyslot.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
         .MinimumLevel.Debug()
         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
         .Enrich.WithProperty("Version", "1.0.0")
         .Enrich.With(new LogEnricher())
         .Enrich.FromLogContext()
         .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
          outputTemplate: "{Timestamp:HH:mm} ({Version}) [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
         .CreateLogger();

            try
            {
                Log.Debug("Starting Book My Slot web host");
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
