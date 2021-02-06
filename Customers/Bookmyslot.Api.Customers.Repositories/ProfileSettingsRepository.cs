﻿using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
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

        public async Task<Response<bool>> UpdateProfileSettings(ProfileSettingsModel profileSettingsModel, string customerId)
        {
            var parameters = new {customerId = customerId, FirstName = profileSettingsModel.FirstName, LastName = profileSettingsModel.LastName, 
                Gender = profileSettingsModel.Gender,  BioHeadLine = profileSettingsModel.BioHeadLine, ModifiedDateUtc = DateTime.UtcNow };
            var sql = CustomerTableQueries.UpdateProfileSettingQuery;

            await this.dbInterceptor.GetQueryResults("UpdateProfileSettings", parameters, () => this.connection.QueryAsync<RegisterCustomerEntity>(sql, parameters));

            return new Response<bool>() { Result = true };
        }
    }
}
