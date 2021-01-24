using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace Bookmyslot.Api.Common.Logging.Enrichers
{
    public class UtcTimestampEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory lepf)
        {
            logEvent.AddPropertyIfAbsent(
              lepf.CreateProperty("UtcTimestamp", logEvent.Timestamp.UtcDateTime));
        }
    }
}
