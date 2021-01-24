using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace Bookmyslot.Api.Common.Logging.Enrichers
{
    public class ThreadLogEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
               "ThreadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}
