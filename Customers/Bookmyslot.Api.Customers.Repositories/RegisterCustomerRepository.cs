using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories.Enitites;
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
        public async Task<Response<string>> CreateCustomer(RegisterCustomerModel registerCustomerModel)
        {
            var resgiterCustomerEntity = EntityFactory.EntityFactory.CreateRegisterCustomerEntity(registerCustomerModel);

            var parameters = new { customerEntity = resgiterCustomerEntity };
            await this.dbInterceptor.GetQueryResults("CreateCustomer", parameters, () => this.connection.InsertAsync<string, RegisterCustomerEntity>(resgiterCustomerEntity));

            return new Response<string>() { Result = registerCustomerModel.Email };
        }

    }
}
