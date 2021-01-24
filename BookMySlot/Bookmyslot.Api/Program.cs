using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Logging;
using Bookmyslot.Api.Common.Logging.Interfaces;
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
            //CreateHostBuilder(args).Build().Run();

            Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Verbose()
          .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
           outputTemplate: "{Timestamp:HH:mm} ({Version}) [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
          .CreateLogger();

            try
            {
                Log.Debug("Starting web host");
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
