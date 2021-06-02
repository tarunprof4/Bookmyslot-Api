using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.EventHandlers
{
    public class SlotScheduledEventHandler : INotificationHandler<SlotScheduledEvent>
    {
        private readonly IEventGridService eventGridService;
        public SlotScheduledEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(SlotScheduledEvent domainEvent, CancellationToken cancellationToken)
        {
            await this.eventGridService.PublishEventAsync(EventConstants.SlotScheduledEvent, domainEvent);
        }
    }
}


