using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.DomainEventHandlers
{

    public class SlotCancelledEventHandler : INotificationHandler<SlotCancelledEvent>
    {
        private readonly IEventGridService eventGridService;
        public SlotCancelledEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(SlotCancelledEvent domainEvent, CancellationToken cancellationToken)
        {
            await this.eventGridService.PublishEventAsync(EventConstants.SlotCancelledEvent, domainEvent);
        }
    }
}
