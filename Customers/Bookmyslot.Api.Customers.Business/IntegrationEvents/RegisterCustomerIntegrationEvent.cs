using Bookmyslot.Api.Common.Contracts.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Business.IntegrationEvents
{
    public class RegisterCustomerIntegrationEvent: IntegrationEvent
    {
        public string FirstName { get; }
        public string LastName { get; }
        public RegisterCustomerIntegrationEvent(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
