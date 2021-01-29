using Bookmyslot.Api.Common.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace Bookmyslot.Api.Common.Logging.Enrichers
{
    public class DefaultLogEnricher : ILogEventEnricher
    {
        private readonly IAppConfiguration appConfiguration;
        private readonly IHttpContextAccessor httpContextAccessor;
        public DefaultLogEnricher(IAppConfiguration appConfiguration, IHttpContextAccessor IHttpContextAccessor)
        {
            this.appConfiguration = appConfiguration;
            this.httpContextAccessor = IHttpContextAccessor;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Version", this.appConfiguration.AppVersion));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Level", logEvent.Level));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UtcTimestamp", logEvent.Timestamp.UtcDateTime));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestId", this.httpContextAccessor.HttpContext?.Request.Headers[LogConstants.RequestId]));
        }
    }
}
