using Bookmyslot.SharedKernel.Event;
using System.Collections.Generic;

namespace Bookmyslot.SharedKernel
{
    public class BaseEventModel
    {
        public string EventId { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}
