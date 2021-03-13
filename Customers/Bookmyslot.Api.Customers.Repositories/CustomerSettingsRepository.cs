using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Bookmyslot.Api.Customers.Repositories.ModelFactory;
using Bookmyslot.Api.Customers.Repositories.Queries;
using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Repositories
{
    public class CustomerSettingsRepository : ICustomerSettingsRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public CustomerSettingsRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }

        public async Task<Response<CustomerSettingsModel>> GetCustomerSettings(string customerId)
        {
            var parameters = new { customerId = customerId };
            var sql = CustomerTableQueries.GetCustomerSettingsQuery;
            var customerSettingsEntity = await this.dbInterceptor.GetQueryResults("GetCustomerSettings", parameters, () => this.connection.QueryFirstOrDefaultAsync<CustomerSettingsEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerSettingsModelResponse(customerSettingsEntity);
        }

     

        public async Task<Response<bool>> UpdateCustomerSettings(string customerId, CustomerSettingsModel customerSettingsModel)
        {
            var parameters = new
            {
                customerId = customerId,
                timeZone = customerSettingsModel.TimeZone,
                ModifiedDateUtc = DateTime.UtcNow
            };

            var sql = CustomerTableQueries.InsertOrUpdateCustomerSettingsQuery;

            await this.dbInterceptor.GetQueryResults("UpdateCustomerSettings", parameters, () => this.connection.QueryAsync<CustomerSettingsEntity>(sql, parameters));

            return new Response<bool>() { Result = true };
        }

      
    }
}
