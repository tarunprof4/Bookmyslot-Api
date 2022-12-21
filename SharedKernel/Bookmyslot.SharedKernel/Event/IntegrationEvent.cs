using System;

namespace Bookmyslot.SharedKernel.Event
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
