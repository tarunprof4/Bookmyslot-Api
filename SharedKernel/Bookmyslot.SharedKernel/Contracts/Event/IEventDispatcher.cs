using Bookmyslot.SharedKernel.Event;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.SharedKernel.Contracts.Event
{
    public interface IEventDispatcher
    {
        Task DispatchEvents(List<BaseDomainEvent> domainEvents);
    }
}
