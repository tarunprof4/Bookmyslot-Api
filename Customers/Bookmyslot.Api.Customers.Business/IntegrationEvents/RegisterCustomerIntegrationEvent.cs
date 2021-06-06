using Bookmyslot.Api.Common.Contracts.Event;

namespace Bookmyslot.Api.Customers.Business.IntegrationEvents
{
    public class RegisterCustomerIntegrationEvent: IntegrationEvent
    {
        public string FirstName { get; }
        public string LastName { get; }

        public string Email { get; }
        public RegisterCustomerIntegrationEvent(string firstName, string lastName, string email)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }
    }
}
