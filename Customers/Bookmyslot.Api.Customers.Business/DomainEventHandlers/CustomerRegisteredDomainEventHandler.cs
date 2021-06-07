using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Customers.Business.IntegrationEvents;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.Customers.Domain.DomainEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.DomainEventHandlers
{

    public class CustomerRegisteredDomainEventHandler : INotificationHandler<CustomerRegisteredDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        public CustomerRegisteredDomainEventHandler(IEventGridService eventGridService)
        {
            this.eventGridService = eventGridService;
        }

        public async Task Handle(CustomerRegisteredDomainEvent customerRegisteredDomainEvent, CancellationToken cancellationToken)
        {
            var registerCustomerModel = customerRegisteredDomainEvent.RegisterCustomerModel;
            var registerCustomerIntegrationEvent = new RegisterCustomerIntegrationEvent(registerCustomerModel);

            await this.eventGridService.PublishEventAsync(EventConstants.CustomerRegisteredEvent, registerCustomerIntegrationEvent);
        }
    }
}
