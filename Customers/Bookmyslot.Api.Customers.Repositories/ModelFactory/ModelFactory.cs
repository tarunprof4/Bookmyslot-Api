using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Repositories.ModelFactory
{
    internal class ModelFactory
    {
        internal static CustomerModel CreateCustomerModel(CustomerEntity customerEntity)
        {
            return new CustomerModel()
            {
                Email = customerEntity.Email,
                Id= customerEntity.UniqueId,
                FirstName = customerEntity.FirstName,
                LastName = customerEntity.LastName,
                Gender = customerEntity.Gender,
                BioHeadLine = customerEntity.BioHeadLine,
                CreatedDateUtc = customerEntity.CreatedDateUtc
            };
        }


        internal static List<CustomerModel> CreateCustomerModels(IEnumerable<CustomerEntity> customerEntities)
        {
            List<CustomerModel> customerModels = new List<CustomerModel>();
            foreach (var customerEntity in customerEntities)
            {
                customerModels.Add(CreateCustomerModel(customerEntity));
            }
            return customerModels;
        }
    }
}
