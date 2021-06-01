using Bookmyslot.Api.Common.Contracts.Event;
using System.Collections.Generic;

namespace Bookmyslot.Api.Common.Contracts
{
    public class BaseEventModel
    {
        public string EventId { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}
