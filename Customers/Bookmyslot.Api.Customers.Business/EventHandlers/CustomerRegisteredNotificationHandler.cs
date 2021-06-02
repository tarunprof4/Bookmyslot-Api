using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.Customers.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.EventHandlers
{

    public class CustomerRegisteredNotificationHandler : INotificationHandler<CustomerRegisteredDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        public CustomerRegisteredNotificationHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(CustomerRegisteredDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            await this.eventGridService.PublishEventAsync(EventConstants.CustomerRegisteredEvent, domainEvent);
        }
    }
}
