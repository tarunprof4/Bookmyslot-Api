using Bookmyslot.Api.SlotScheduler.Contracts;
using Serilog.Core;
using Serilog.Events;

namespace Bookmyslot.Api.Common.Logging.LogContexts
{
   
    public class CustomLogContext : ILogEventEnricher
    {
        private object customObject;
        public CustomLogContext(object customObject)
        {
            this.customObject = customObject;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (this.customObject != null)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("customObject", this.customObject, true));
            }
        }
    }
}
