using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
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
    public class AdditionalProfileSettingsRepository : IAdditionalProfileSettingsRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public AdditionalProfileSettingsRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }

        public async Task<Response<AdditionalProfileSettingsModel>> GetAdditionalProfileSettingsByCustomerId(string customerId)
        {
            var parameters = new { customerId = customerId };
            var sql = RegisterCustomerTableQueries.GetAdditionalProfileSettingsByCustomerIdQuery;
            var registerCustomerEntity = await this.dbInterceptor.GetQueryResults("GetAdditionalProfileSettings", parameters, () => this.connection.QueryFirstOrDefaultAsync<RegisterCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateAdditionalProfileSettingsModelResponse(registerCustomerEntity);
        }

        public async Task<Response<bool>> UpdateAdditionalProfileSettings(string customerId, AdditionalProfileSettingsModel additionalProfileSettingsModel)
        {
            var parameters = new
            {
                customerId = customerId,
                bioHeadLine = additionalProfileSettingsModel.BioHeadLine,
                modifiedDateUtc = DateTime.UtcNow
            };
            var sql = RegisterCustomerTableQueries.UpdateAdditionalProfileSettingQuery;

            await this.dbInterceptor.GetQueryResults("UpdateAdditionalProfileSettings", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Response<bool>() { Result = true };
        }
    }
}
