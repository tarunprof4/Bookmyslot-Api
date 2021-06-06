using Bookmyslot.BackgroundTasks.Api.Contracts.Configuration;
using Bookmyslot.BackgroundTasks.Api.Logging.Constants;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Bookmyslot.BackgroundTasks.Api.Logging.Enrichers
{
    public class DefaultLogEnricher : ILogEventEnricher
    {
        private readonly AppConfiguration appConfiguration;
        private readonly IHttpContextAccessor httpContextAccessor;
        public DefaultLogEnricher(AppConfiguration appConfiguration, IHttpContextAccessor IHttpContextAccessor)
        {
            this.appConfiguration = appConfiguration;
            this.httpContextAccessor = IHttpContextAccessor;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Version", this.appConfiguration.AppVersion));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Level", logEvent.Level));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UtcTimestamp", logEvent.Timestamp.UtcDateTime));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CoorelationId", this.httpContextAccessor.HttpContext?.Request.Headers[LogConstants.CoorelationId]));
        }
    }
}
