﻿using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Cache.Repositories.Enitites;
using Bookmyslot.Api.Cache.Repositories.ModelFactory;
using Bookmyslot.Api.Cache.Repositories.Queries;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public CacheRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }


        public async Task<Response<bool>> CreateCache(CacheModel cacheModel)
        {
            var cacheEntity = EntityFactory.EntityFactory.CreateCacheKeyEntity(cacheModel);
            var parameters = new { cacheEntity = cacheEntity };

            await this.dbInterceptor.GetQueryResults("CreateCache", parameters, () => this.connection.InsertAsync<CacheEntity>(cacheEntity));

            return new Response<bool>() { Result = true };
        }

        public async Task<Response<string>> GetCache(string cacheType, string cacheKey)
        {
            var sql = CacheTableQueries.GetCacheQuery;
            var parameters = new { cacheType = cacheType, cacheKey = cacheKey };

            var cacheEntity = await this.dbInterceptor.GetQueryResults("GetCache", parameters, () => this.connection.QueryFirstOrDefaultAsync<CacheEntity>(sql, parameters));

            return ResponseModelFactory.CreateCacheValueResponse(cacheEntity);
        }
    }
}
