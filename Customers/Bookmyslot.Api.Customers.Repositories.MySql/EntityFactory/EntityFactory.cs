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
                Id = CreateUniqueId(),
                FirstName = registerCustomerModel.FirstName,
                LastName = registerCustomerModel.LastName,
                UserName = CreateUserName(registerCustomerModel.FirstName),
                Email = registerCustomerModel.Email.ToLowerInvariant(),
                Provider = registerCustomerModel.Provider,
                CreatedDateUtc = DateTime.UtcNow,
                IsVerified = registerCustomerModel.IsVerified,
            };
        }

        private static string CreateUniqueId()
        {
            var guid1 = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var guid2 = Guid.NewGuid().ToString().Substring(0,4).Replace("-", string.Empty);
            
            var uniqueId = string.Format("{0}{1}", guid1, guid2);
            return uniqueId;
        }

        private static string CreateUserName(string firstName)
        {
            var guid1 = Guid.NewGuid().ToString().Replace("-", string.Empty);

            var userName = string.Format("{0}{1}", firstName, guid1);
            return userName.ToLowerInvariant();
        }
    }
}
