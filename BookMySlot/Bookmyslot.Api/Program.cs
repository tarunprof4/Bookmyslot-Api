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
            ILoggerService loggerService = new LoggerService();
            try
            {
                loggerService.LogDebug("Starting Book My Slot web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                loggerService.LogFatal("Host terminated unexpectedly", ex);
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
