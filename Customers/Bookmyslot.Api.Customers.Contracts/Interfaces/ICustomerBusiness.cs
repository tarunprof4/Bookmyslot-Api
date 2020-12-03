using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerBusiness
    {
        Customer GetCustomer(string email);
    }
}
