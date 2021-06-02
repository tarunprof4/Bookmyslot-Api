using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.EventHandlers
{

    public class SlotMeetingInformationRequestedEventHandler : INotificationHandler<SlotMeetingInformationRequestedEvent>
    {
        private readonly IEventGridService eventGridService;
        public SlotMeetingInformationRequestedEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(SlotMeetingInformationRequestedEvent domainEvent, CancellationToken cancellationToken)
        {
            await this.eventGridService.PublishEventAsync(EventConstants.SlotMeetingInformationRequestedEvent, domainEvent);
        }
    }
}
