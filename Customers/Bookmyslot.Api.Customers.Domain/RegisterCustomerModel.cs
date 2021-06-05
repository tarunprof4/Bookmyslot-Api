using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Domain.DomainEvents;

namespace Bookmyslot.Api.Customers.Domain
{
    public class RegisterCustomerModel : BaseEventModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Provider { get; set; }

        public bool IsVerified { get; set; }

        public void RegisterCustomer()
        {
            Events.Add(new CustomerRegisteredDomainEvent(this));
        }
    }
}
