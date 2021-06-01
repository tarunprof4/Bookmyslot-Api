using Bookmyslot.Api.Common.Contracts.Event;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
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
