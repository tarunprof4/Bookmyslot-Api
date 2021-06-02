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
            // do not try to refactor this code - its needed else stackoverflow exception will happen
            var events = domainEvents.ToArray();
            domainEvents.Clear();
            foreach (var domainEvent in events)
            {
                await this.mediator.Publish(domainEvent);
            }
        }

    }
}
