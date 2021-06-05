using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using Bookmyslot.Api.SlotScheduler.Domain.DomainEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.DomainEventHandlers
{
    public class SlotScheduledEventHandler : INotificationHandler<SlotScheduledDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        public SlotScheduledEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(SlotScheduledDomainEvent slotScheduledDomainEvent, CancellationToken cancellationToken)
        {
            var bookedBy = slotScheduledDomainEvent.BookedByCustomerModel.GetCustomerFullName();
            var slotScheduledIntegrationEvent = new SlotScheduledIntegrationEvent(slotScheduledDomainEvent.SlotModel, bookedBy);
            
            await this.eventGridService.PublishEventAsync(EventConstants.SlotScheduledEvent, slotScheduledIntegrationEvent);
        }
    }
}


