using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Contracts.Event
{
    public interface IEventDispatcher
    {
        Task DispatchEvents(List<BaseDomainEvent> domainEvents);
    }
}
