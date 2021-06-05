using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents;
using Bookmyslot.Api.SlotScheduler.Domain.DomainEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.DomainEventHandlers
{

    public class SlotMeetingInformationRequestedEventHandler : INotificationHandler<SlotMeetingInformationRequestedDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        public SlotMeetingInformationRequestedEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(SlotMeetingInformationRequestedDomainEvent slotMeetingInformationRequestedDomainEvent, CancellationToken cancellationToken)
        {
            var resendTo = slotMeetingInformationRequestedDomainEvent.ResendToCustomerSummaryModel.FullName;
            var slotScheduledIntegrationEvent = new SlotMeetingInformationRequestedIntegrationEvent(slotMeetingInformationRequestedDomainEvent.SlotModel, resendTo);
            await this.eventGridService.PublishEventAsync(EventConstants.SlotMeetingInformationRequestedEvent, slotScheduledIntegrationEvent);
        }
    }
}
