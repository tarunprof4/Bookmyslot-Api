﻿using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace Bookmyslot.Api.Common.Logging.Enrichers
{
    public class StaticDefaultLogEnricher : ILogEventEnricher
    {
        private readonly string appVersion;
        public StaticDefaultLogEnricher(string appVersion)
        {
            this.appVersion = appVersion;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Version", this.appVersion));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Level", logEvent.Level));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UtcTimestamp", logEvent.Timestamp.UtcDateTime));
        }
    }
}
