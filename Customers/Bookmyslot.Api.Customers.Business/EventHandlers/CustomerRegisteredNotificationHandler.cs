using Bookmyslot.Api.Customers.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.EventHandlers
{

    public class CustomerRegisteredNotificationHandler : INotificationHandler<CustomerRegisteredDomainEvent>
    {

        public CustomerRegisteredNotificationHandler()
        {

        }

        public async Task Handle(CustomerRegisteredDomainEvent domainEvent, CancellationToken cancellationToken)
        {

            
        }
    }
}
