using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Bookmyslot.Api.Customers.Repositories.ModelFactory;
using Bookmyslot.Api.Customers.Repositories.Queries;
using Bookmyslot.SharedKernel.Contracts.Database;
using Bookmyslot.SharedKernel.ValueObject;
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

        public async Task<Result<CustomerSettingsModel>> GetCustomerSettings(string customerId)
        {
            var parameters = new { customerId = customerId };
            var sql = CustomerSettingsTableQueries.GetCustomerSettingsQuery;
            var customerSettingsEntity = await this.dbInterceptor.GetQueryResults("GetCustomerSettings", parameters, () => this.connection.QueryFirstOrDefaultAsync<CustomerSettingsEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerSettingsModelResponse(customerSettingsEntity);
        }



        public async Task<Result<bool>> UpdateCustomerSettings(string customerId, CustomerSettingsModel customerSettingsModel)
        {
            var parameters = new
            {
                customerId = customerId,
                Country = customerSettingsModel.Country,
                timeZone = customerSettingsModel.TimeZone,
                ModifiedDateUtc = DateTime.UtcNow
            };

            var sql = CustomerSettingsTableQueries.InsertOrUpdateCustomerSettingsQuery;

            await this.dbInterceptor.GetQueryResults("UpdateCustomerSettings", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Result<bool>() { Value = true };
        }


    }
}
