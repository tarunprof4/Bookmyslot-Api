using Bookmyslot.Api.Common.Logging.Contracts;
using Serilog.Core;
using Serilog.Events;

namespace Bookmyslot.Api.Common.Logging.LogContexts
{
    public class DatabaseResponseLogContext : ILogEventEnricher
    {
        private readonly DatabaseResponseLog databaseResponseLog;
        public DatabaseResponseLogContext(DatabaseResponseLog databaseResponseLog)
        {
            this.databaseResponseLog = databaseResponseLog;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (this.databaseResponseLog != null)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogConstants.DataBaseRespponseLogContext, this.databaseResponseLog, true));
            }
        }
    }
}
