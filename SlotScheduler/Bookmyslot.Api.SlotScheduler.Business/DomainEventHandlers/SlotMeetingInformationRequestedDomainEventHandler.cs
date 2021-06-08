using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using Bookmyslot.Api.SlotScheduler.Domain.DomainEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.DomainEventHandlers
{

    public class SlotMeetingInformationRequestedDomainEventHandler : INotificationHandler<SlotMeetingInformationRequestedDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        public SlotMeetingInformationRequestedDomainEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(SlotMeetingInformationRequestedDomainEvent slotMeetingInformationRequestedDomainEvent, CancellationToken cancellationToken)
        {
            var slotScheduledIntegrationEvent = new SlotMeetingInformationRequestedIntegrationEvent(slotMeetingInformationRequestedDomainEvent.SlotModel,
                slotMeetingInformationRequestedDomainEvent.ResendToCustomerModel);
            await this.eventGridService.PublishEventAsync(slotScheduledIntegrationEvent);
        }
    }
}
