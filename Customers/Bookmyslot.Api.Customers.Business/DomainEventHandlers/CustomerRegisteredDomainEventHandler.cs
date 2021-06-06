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
            var RegisterCustomerIntegrationEvent = new RegisterCustomerIntegrationEvent(registerCustomerModel.FirstName,
                registerCustomerModel.LastName, registerCustomerModel.Email);

            await this.eventGridService.PublishEventAsync(EventConstants.CustomerRegisteredEvent, RegisterCustomerIntegrationEvent);
        }
    }
}
