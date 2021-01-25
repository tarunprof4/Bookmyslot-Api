using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
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
        private readonly ISqlInterceptor sqlInterceptor;

        public CustomerRepository(IDbConnection connection, ISqlInterceptor sqlInterceptor)
        {
            this.connection = connection;
            this.sqlInterceptor = sqlInterceptor;
        }
        public async Task<Response<string>> CreateCustomer(CustomerModel customerModel)
        {
            var customerEntity = EntityFactory.EntityFactory.CreateCustomerEntity(customerModel);
            
            var parameters = new { customerEntity = customerEntity };
            await this.sqlInterceptor.GetQueryResults("CreateCustomer", parameters, () => this.connection.InsertAsync<string, CustomerEntity>(customerEntity));

            return new Response<string>() { Result = customerModel.Email };
        }

      
        public async Task<Response<IEnumerable<CustomerModel>>> GetAllCustomers()
        {
            var customerEntities = await this.connection.GetListAsync<CustomerEntity>();
            var customerModels = ModelFactory.ModelFactory.CreateCustomerModels(customerEntities);
            if (customerModels.Count == 0)
            {
                return Response<IEnumerable<CustomerModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound });
            }

            return new Response<IEnumerable<CustomerModel>>() { Result = customerModels };
        }

        public async Task<Response<CustomerModel>> GetCustomerByEmail(string email)
        {
            var parameters = new { Email = email };
            var sql = CustomerTableQueries.GetCustomerByEmailsQuery;
            var customerEntity = await this.sqlInterceptor.GetQueryResults("GetCustomerByEmail", parameters, () => this.connection.QueryFirstOrDefaultAsync<CustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerModelResponse(customerEntity);
        }

        


        public async Task<Response<CustomerModel>> GetCustomerById(string customerId)
        {
            var parameters = new { customerId = customerId };

            var customerEntity = await this.sqlInterceptor.GetQueryResults("GetCustomerById", parameters, () => this.connection.GetAsync<CustomerEntity>(customerId));

            return ResponseModelFactory.CreateCustomerModelResponse(customerEntity);
        }

        public async Task<Response<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds)
        {
            var parameters = new { CustomerIds = customerIds };
            var sql = CustomerTableQueries.GetCustomersByCustomerIdsQuery;

            var customerEntities = await this.sqlInterceptor.GetQueryResults("GetCustomersByCustomerIds", parameters, () => this.connection.QueryAsync<CustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerModelsResponse(customerEntities);
        }

    

        public async Task<Response<bool>> UpdateCustomer(CustomerModel customerModel)
        {
            var customerEntity = EntityFactory.EntityFactory.UpdateCustomerEntity(customerModel);
            
            var parameters = new { customerEntity = customerEntity };
            await this.sqlInterceptor.GetQueryResults("UpdateCustomer", parameters, () => this.connection.UpdateAsync<CustomerEntity>(customerEntity));

            return new Response<bool>() { Result = true };
        }

       
    }
}
