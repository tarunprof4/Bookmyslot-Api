using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using Bookmyslot.Api.SlotScheduler.Domain.DomainEvents;
using Bookmyslot.SharedKernel.Contracts.EventGrid;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.DomainEventHandlers
{

    public class SlotCancelledDomainEventHandler : INotificationHandler<SlotCancelledDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        public SlotCancelledDomainEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(SlotCancelledDomainEvent slotCancelledDomainEvent, CancellationToken cancellationToken)
        {
            var slotCancelledIntegrationEvent = new SlotCancelledIntegrationEvent(slotCancelledDomainEvent.CancelledSlotModel,
                slotCancelledDomainEvent.CancelledByCustomerModel);

            await this.eventGridService.PublishEventAsync(slotCancelledIntegrationEvent);
        }
    }
}
