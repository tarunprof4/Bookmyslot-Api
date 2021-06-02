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
            //foreach (var entity in entitiesWithEvents)
            //{
            //    var events = entity.Events.ToArray();
            //    entity.Events.Clear();
            //    foreach (var domainEvent in events)
            //    {
            //        await _mediator.Publish(domainEvent).ConfigureAwait(false);
            //    }
            //}

            foreach (var domainEvent in domainEvents)
            {
                await this.mediator.Publish(domainEvent);
            }
            domainEvents.Clear();
        }

     
    }
}
