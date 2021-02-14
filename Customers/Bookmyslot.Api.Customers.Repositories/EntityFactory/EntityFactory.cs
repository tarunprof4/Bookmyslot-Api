using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using System;

namespace Bookmyslot.Api.Customers.Repositories.EntityFactory
{
    internal class EntityFactory
    {
        internal static RegisterCustomerEntity CreateRegisterCustomerEntity(RegisterCustomerModel registerCustomerModel)
        {
            return new RegisterCustomerEntity()
            {
                UniqueId = Guid.NewGuid().ToString().Replace("-", string.Empty),
                FirstName = registerCustomerModel.FirstName,
                LastName = registerCustomerModel.LastName,
                Gender = registerCustomerModel.Gender,
                UserName = registerCustomerModel.UserName,
                Email = registerCustomerModel.Email,
                PhoneNumber = registerCustomerModel.PhoneNumber,
                BioHeadLine = registerCustomerModel.BioHeadLine,
                Provider = registerCustomerModel.Provider,
                PhotoUrl = registerCustomerModel.PhotoUrl,
                CreatedDateUtc = DateTime.UtcNow
            };
        }

       
    }
}
