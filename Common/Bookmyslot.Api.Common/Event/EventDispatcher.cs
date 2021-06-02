using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Common.Contracts.Event.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Event
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IMediator mediator;

        public EventDispatcher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task DispatchEvents(List<BaseDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await this.mediator.Publish(domainEvent);
            }
            domainEvents.Clear();
        }

     
    }
}
