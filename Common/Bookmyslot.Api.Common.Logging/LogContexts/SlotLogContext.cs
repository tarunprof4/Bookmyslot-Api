using Bookmyslot.Api.SlotScheduler.Contracts;
using Serilog.Core;
using Serilog.Events;

namespace Bookmyslot.Api.Common.Logging.LogContexts
{
   
    public class SlotLogContext : ILogEventEnricher
    {
        private SlotModel slotModel;
        public SlotLogContext(SlotModel slotModel)
        {
            this.slotModel = slotModel;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (this.slotModel != null)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogConstants.SlotLogContext, this.slotModel, true));
            }
        }
    }
}
