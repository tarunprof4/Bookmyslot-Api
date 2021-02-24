using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Repositories.ModelFactory
{
    internal class ModelFactory
    {
        internal static CustomerModel CreateCustomerModel(RegisterCustomerEntity registerCustomerEntity)
        {
            return new CustomerModel()
            {
                Id= registerCustomerEntity.UniqueId,
                FirstName = registerCustomerEntity.FirstName,
                LastName = registerCustomerEntity.LastName,
                BioHeadLine = registerCustomerEntity.BioHeadLine,
            };
        }

        internal static ProfileSettingsModel CreateProfileSettingsModel(RegisterCustomerEntity registerCustomerEntity)
        {
            return new ProfileSettingsModel()
            {
                Email = registerCustomerEntity.Email,
                FirstName = registerCustomerEntity.FirstName,
                LastName = registerCustomerEntity.LastName,
                Gender = registerCustomerEntity.Gender
            };
        }


        internal static List<CustomerModel> CreateCustomerModels(IEnumerable<RegisterCustomerEntity> registerCustomerEntities)
        {
            List<CustomerModel> customerModels = new List<CustomerModel>();
            foreach (var registerCustomerEntity in registerCustomerEntities)
            {
                customerModels.Add(CreateCustomerModel(registerCustomerEntity));
            }
            return customerModels;
        }
    }
}
