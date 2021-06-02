using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Contracts.Event.Interfaces
{
    public interface IEventDispatcher
    {
        Task DispatchEvents(List<BaseDomainEvent> domainEvents);
    }
}
