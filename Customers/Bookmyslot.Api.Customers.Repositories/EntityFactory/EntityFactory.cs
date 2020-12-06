using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Repositories.EntityFactory
{
    internal class EntityFactory
    {
        internal static CustomerEntity CreateCustomerEntity(CustomerModel customerModel)
        {
            return new CustomerEntity()
            {
                Email = customerModel.Email,
                GenderPrefix = customerModel.GenderPrefix,
                FirstName = customerModel.FirstName,
                MiddleName = customerModel.MiddleName,
                LastName = customerModel.LastName,
                Gender = customerModel.Gender,
               // ModifiedDate = DateTime.MinValue
            };
        }

        internal static CustomerEntity UpdateCustomerEntity(CustomerModel customerModel)
        {
            var customerEntity = CreateCustomerEntity(customerModel);
           // customerEntity.ModifiedDate = DateTime.UtcNow;
            return customerEntity;
        }
    }
}
