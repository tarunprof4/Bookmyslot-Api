using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.Customers.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Bookmyslot.Api.Customers.Business.IntegrationEvents;

namespace Bookmyslot.Api.Customers.Business.DomainEventHandlers
{

    public class CustomerRegisteredNotificationHandler : INotificationHandler<CustomerRegisteredDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        public CustomerRegisteredNotificationHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(CustomerRegisteredDomainEvent customerRegisteredDomainEvent, CancellationToken cancellationToken)
        {
            var registerCustomerModel = customerRegisteredDomainEvent.RegisterCustomerModel;
            var RegisterCustomerIntegrationEvent = new RegisterCustomerIntegrationEvent(registerCustomerModel.FirstName,
                registerCustomerModel.LastName);

            await this.eventGridService.PublishEventAsync(EventConstants.CustomerRegisteredEvent, RegisterCustomerIntegrationEvent);
        }
    }
}
