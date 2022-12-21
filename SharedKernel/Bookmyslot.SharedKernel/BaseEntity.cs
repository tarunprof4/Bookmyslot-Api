using Bookmyslot.SharedKernel.Event;
using System.Collections.Generic;

namespace Bookmyslot.SharedKernel
{
    public class BaseEntity<T>
    {
        public T EventId { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}
