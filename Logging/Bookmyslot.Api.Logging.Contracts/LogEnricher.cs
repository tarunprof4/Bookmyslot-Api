using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace Bookmyslot.Api.Logging.Contracts
{
    public class LogEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
               "ThreadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}
