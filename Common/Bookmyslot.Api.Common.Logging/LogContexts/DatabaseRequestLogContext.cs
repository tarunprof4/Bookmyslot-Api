using Bookmyslot.Api.Common.Logging.Contracts;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Logging.LogContexts
{
   
    public class DatabaseRequestLogContext : ILogEventEnricher
    {
        private readonly DatabaseRequestLog databaseRequestLog;
        public DatabaseRequestLogContext(DatabaseRequestLog databaseRequestLog)
        {
            this.databaseRequestLog = databaseRequestLog;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (this.databaseRequestLog != null)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogConstants.DataBaseRequestLogContext, this.databaseRequestLog, true));
            }
        }
    }
}
