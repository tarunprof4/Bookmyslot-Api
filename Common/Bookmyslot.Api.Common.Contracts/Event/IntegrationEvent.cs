using System;

namespace Bookmyslot.Api.Common.Contracts.Event
{
    public class IntegrationEvent
    {
        public IntegrationEvent(string eventType)
        {
            Id = Guid.NewGuid();
            EventType = eventType;
            CreationDate = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public string EventType { get; private set; }

        public DateTime CreationDate { get; private set; }
    }
}
