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
    public class CustomerAdditionalInformationRepository : ICustomerAdditionalInformationRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public CustomerAdditionalInformationRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }

        public async Task<Response<CustomerAdditionalInformationModel>> GetCustomerAdditionalInformation(string customerId)
        {
            var parameters = new { customerId = customerId };
            var sql = CustomerTableQueries.GetCustomerAdditionInformationQuery;
            var customerAdditionalInformationEntity = await this.dbInterceptor.GetQueryResults("GetCustomerAdditionalInformation", parameters, () => this.connection.QueryFirstOrDefaultAsync<CustomerAdditionalInformationEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomerAdditionalInformationModelResponse(customerAdditionalInformationEntity);
        }

        public async Task<Response<bool>> UpdateCustomerAdditionalInformation(string customerId, CustomerAdditionalInformationModel customerAdditionalInformationModel)
        {
            var parameters = new
            {
                customerId = customerId,
                timeZone = customerAdditionalInformationModel.TimeZone,
                ModifiedDateUtc = DateTime.UtcNow
            };

            var sql = CustomerTableQueries.InsertOrUpdateCustomerAdditionInformationQuery;

            await this.dbInterceptor.GetQueryResults("UpdateCustomerTimeZoneInformation", parameters, () => this.connection.QueryAsync<CustomerAdditionalInformationEntity>(sql, parameters));

            return new Response<bool>() { Result = true };
        }
    }
}
