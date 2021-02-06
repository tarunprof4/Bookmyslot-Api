using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Bookmyslot.Api.Customers.Repositories.ModelFactory;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
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
            var parameters = new { Email = email };
            var sql = CustomerTableQueries.GetCustomerByEmailsQuery;
            var customerEntity = await this.dbInterceptor.GetQueryResults("GetCustomerByEmail", parameters, () => this.connection.QueryFirstOrDefaultAsync<CustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerModelResponse(customerEntity);
        }

        
        public async Task<Response<CustomerModel>> GetCustomerById(string customerId)
        {
            var parameters = new { customerId = customerId };

            var customerEntity = await this.dbInterceptor.GetQueryResults("GetCustomerById", parameters, () => this.connection.GetAsync<CustomerEntity>(customerId));

            return ResponseModelFactory.CreateCustomerModelResponse(customerEntity);
        }

        public async Task<Response<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds)
        {
            var parameters = new { CustomerIds = customerIds };
            var sql = CustomerTableQueries.GetCustomersByCustomerIdsQuery;

            var customerEntities = await this.dbInterceptor.GetQueryResults("GetCustomersByCustomerIds", parameters, () => this.connection.QueryAsync<CustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerModelsResponse(customerEntities);
        }

    

       
    }
}
