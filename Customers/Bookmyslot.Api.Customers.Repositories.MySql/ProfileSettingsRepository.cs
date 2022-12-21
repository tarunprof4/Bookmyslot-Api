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
    public class ProfileSettingsRepository : IProfileSettingsRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public ProfileSettingsRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }

        public async Task<Result<ProfileSettingsModel>> GetProfileSettingsByCustomerId(string customerId)
        {
            var parameters = new { customerId = customerId };
            var sql = RegisterCustomerTableQueries.GetProfileSettingsByCustomerIdQuery;
            var registerCustomerEntity = await this.dbInterceptor.GetQueryResults("GetProfileSettingsByEmail", parameters, () => this.connection.QueryFirstOrDefaultAsync<RegisterCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateProfileSettingsModelResponse(registerCustomerEntity);
        }

        public async Task<Result<bool>> UpdateProfilePicture(string customerId, string profilePictureUrl)
        {
            var parameters = new
            {
                customerId = customerId,
                profilePictureUrl = profilePictureUrl,
                modifiedDateUtc = DateTime.UtcNow
            };
            var sql = RegisterCustomerTableQueries.UpdateProfilePictureQuery;

            await this.dbInterceptor.GetQueryResults("UpdateProfilePicture", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Result<bool>() { Value = true };
        }

        public async Task<Result<bool>> UpdateProfileSettings(ProfileSettingsModel profileSettingsModel, string customerId)
        {
            var parameters = new
            {
                customerId = customerId,
                FirstName = profileSettingsModel.FirstName,
                LastName = profileSettingsModel.LastName,
                Gender = profileSettingsModel.Gender,
                ModifiedDateUtc = DateTime.UtcNow
            };
            var sql = RegisterCustomerTableQueries.UpdateProfileSettingQuery;

            await this.dbInterceptor.GetQueryResults("UpdateProfileSettings", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Result<bool>() { Value = true };
        }
    }
}
