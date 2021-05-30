using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories.Queries;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Repositories
{
    public class RegisterCustomerRepository : IRegisterCustomerRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public RegisterCustomerRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }
    
        public async Task<Response<string>> RegisterCustomer(RegisterCustomerModel registerCustomerModel)
        {
            var registerCustomerEntity = EntityFactory.EntityFactory.CreateRegisterCustomerEntity(registerCustomerModel);
            var sql = RegisterCustomerTableQueries.RegisterCustomerQuery;
            var parameters = new
            {
                Id = registerCustomerEntity.Id,
                FirstName = registerCustomerEntity.FirstName,
                LastName = registerCustomerEntity.LastName,
                UserName = registerCustomerEntity.UserName,
                Email = registerCustomerEntity.Email,
                Provider = registerCustomerEntity.Provider,
                CreatedDateUtc = registerCustomerEntity.CreatedDateUtc,
                IsVerified = registerCustomerEntity.IsVerified
            };


            await this.dbInterceptor.GetQueryResults("RegisterCustomer", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Response<string>() { Result = registerCustomerModel.Email };
        }



    }
}
