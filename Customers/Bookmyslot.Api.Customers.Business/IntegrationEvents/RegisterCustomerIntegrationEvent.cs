using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.SharedKernel.Event;

namespace Bookmyslot.Api.Customers.Business.IntegrationEvents
{
    public class RegisterCustomerIntegrationEvent : IntegrationEvent
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public RegisterCustomerIntegrationEvent(RegisterCustomerModel registerCustomerModel) :
            base(EventConstants.CustomerRegisteredEvent)
        {

            this.FirstName = registerCustomerModel.FirstName;
            this.LastName = registerCustomerModel.LastName;
            this.Email = registerCustomerModel.Email;
        }
    }
}
