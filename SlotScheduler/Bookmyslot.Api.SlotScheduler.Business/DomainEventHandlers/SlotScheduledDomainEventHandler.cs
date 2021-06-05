using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using Bookmyslot.Api.SlotScheduler.Domain.DomainEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.DomainEventHandlers
{
    public class SlotScheduledDomainEventHandler : INotificationHandler<SlotScheduledDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        public SlotScheduledDomainEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(SlotScheduledDomainEvent slotScheduledDomainEvent, CancellationToken cancellationToken)
        {
            var bookedBy = slotScheduledDomainEvent.BookedByCustomerSummaryModel.FullName;
            var slotScheduledIntegrationEvent = new SlotScheduledIntegrationEvent(slotScheduledDomainEvent.SlotModel, bookedBy);
            await this.eventGridService.PublishEventAsync(EventConstants.SlotScheduledEvent, slotScheduledIntegrationEvent);
        }
    }
}


