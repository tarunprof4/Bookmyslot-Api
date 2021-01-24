using Bookmyslot.Api.Common.Contracts.Interfaces;
using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace Bookmyslot.Api.Common.Logging.Enrichers
{
    public class DefaultLogEnricher : ILogEventEnricher
    {
        private readonly IAppConfiguration appConfiguration;
        public DefaultLogEnricher(IAppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ThreadId", Thread.CurrentThread.ManagedThreadId));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Version", this.appConfiguration.AppVersion));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Level", logEvent.Level));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UtcTimestamp", logEvent.Timestamp.UtcDateTime));
        }
    }
}
