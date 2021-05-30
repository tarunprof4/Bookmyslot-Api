using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Bookmyslot.Api.Customers.Repositories.ModelFactory;
using Bookmyslot.Api.Customers.Repositories.Queries;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public CustomerRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }

      
        public async Task<Response<CustomerModel>> GetCustomerByEmail(string email)
        {
            var parameters = new { Email = email.ToLowerInvariant() };
            var sql = RegisterCustomerTableQueries.GetCustomerByEmailQuery;
            var registerCustomerEntity = await this.dbInterceptor.GetQueryResults("GetCustomerByEmail", parameters, () => this.connection.QueryFirstOrDefaultAsync<RegisterCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerModelResponse(registerCustomerEntity);
        }


        public async Task<Response<string>> GetCustomerIdByEmail(string email)
        {
            var parameters = new { Email = email.ToLowerInvariant() };
            var sql = RegisterCustomerTableQueries.GetCustomerIdByEmailQuery;
            var registerCustomerEntity = await this.dbInterceptor.GetQueryResults("GetCustomerIdByEmail", parameters, () => this.connection.QueryFirstOrDefaultAsync<RegisterCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerIdResponse(registerCustomerEntity);
        }

        public async Task<Response<CustomerModel>> GetCustomerById(string customerId)
        {
            var parameters = new { CustomerId = customerId };
            var sql = RegisterCustomerTableQueries.GetCustomerByIdQuery;

            var registerCustomerEntity = await this.dbInterceptor.GetQueryResults("GetCustomerById", parameters, () => this.connection.QueryFirstOrDefaultAsync<RegisterCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerModelResponse(registerCustomerEntity);
        }

     

        public async Task<Response<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds)
        {
            var parameters = new { CustomerIds = customerIds };
            var sql = RegisterCustomerTableQueries.GetCustomersByCustomerIdsQuery;

            var registerCustomerEntities = await this.dbInterceptor.GetQueryResults("GetCustomersByCustomerIds", parameters, () => this.connection.QueryAsync<RegisterCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerModelsResponse(registerCustomerEntities);
        }

    

       
    }
}
