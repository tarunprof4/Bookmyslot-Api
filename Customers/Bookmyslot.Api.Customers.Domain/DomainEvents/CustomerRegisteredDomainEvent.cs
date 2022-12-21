using Bookmyslot.SharedKernel.Event;

namespace Bookmyslot.Api.Customers.Domain.DomainEvents
{
    public class CustomerRegisteredDomainEvent : BaseDomainEvent
    {
        public RegisterCustomerModel RegisterCustomerModel { get; set; }

        public CustomerRegisteredDomainEvent(RegisterCustomerModel registerCustomerModel)
        {
            this.RegisterCustomerModel = registerCustomerModel;
        }
    }
}
