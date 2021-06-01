using Bookmyslot.Api.Customers.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.EventHandlers
{

    public class CustomerRegisteredNotificationHandler1: INotificationHandler<CustomerRegisteredDomainEvent>
    {

        public CustomerRegisteredNotificationHandler1()
        {

        }

        public async Task Handle(CustomerRegisteredDomainEvent domainEvent, CancellationToken cancellationToken)
        {


        }
    }
}
