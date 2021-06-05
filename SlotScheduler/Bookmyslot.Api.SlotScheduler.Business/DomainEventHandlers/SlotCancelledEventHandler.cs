using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using Bookmyslot.Api.SlotScheduler.Domain.DomainEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.DomainEventHandlers
{

    public class SlotCancelledEventHandler : INotificationHandler<SlotCancelledDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        public SlotCancelledEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(SlotCancelledDomainEvent slotCancelledDomainEvent, CancellationToken cancellationToken)
        {
            var cancelledBy = slotCancelledDomainEvent.CancelledByCustomerSummaryModel.FullName;
            var slotCancelledIntegrationEvent = new SlotCancelledIntegrationEvent(slotCancelledDomainEvent.CancelledSlotModel, cancelledBy);

            await this.eventGridService.PublishEventAsync(EventConstants.SlotCancelledEvent, slotCancelledIntegrationEvent);
        }
    }
}
