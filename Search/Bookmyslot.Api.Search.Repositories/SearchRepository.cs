using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Bookmyslot.SharedKernel.Contracts.Compression;
using Bookmyslot.SharedKernel.Contracts.Database;
using Bookmyslot.SharedKernel.ValueObject;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;
        private readonly ICompression compression;

        public SearchRepository(IDbConnection connection, IDbInterceptor dbInterceptor, ICompression compression)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
            this.compression = compression;
        }

        public async Task<Result<T>> GetPreProcessedSearchedResponse<T>(string searchType, string searchKey)
        {

            var parameters = new { SearchKey = CreateSearchKey(searchType, searchKey) };
            var sql = SearchTableQueries.GetPreProcessedSearchedCustomerQuery;

            var compressedSearchedCustomers = await this.dbInterceptor.GetQueryResults("GetPreProcessedSearchedCustomers", parameters, () => this.connection.QueryFirstOrDefaultAsync<SearchEntity>(sql, parameters));

            if (compressedSearchedCustomers != null)
            {
                var deCompressedSearchCustomerModels = this.compression.Decompress<T>(compressedSearchedCustomers.Value);
                return new Result<T>() { Value = deCompressedSearchCustomerModels };
            }

            return Result<T>.Empty(new List<string>() { AppBusinessMessagesConstants.NoCustomerSearchResults });
        }

        public async Task<Result<bool>> SavePreProcessedSearchedResponse<T>(string searchType, string searchKey, T response)
        {
            var parameters = new
            {
                SearchKey = CreateSearchKey(searchType, searchKey),
                Value = this.compression.Compress(response),
                ModifiedDateUtc = DateTime.UtcNow
            };
            var sql = SearchTableQueries.InsertOrUpdatePreProcessedSearchedCustomerQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SavePreProcessedSearchedCustomers", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Result<bool>() { Value = true };
        }



        private string CreateSearchKey(string searchType, string key)
        {
            var searchKey = string.Format("{0}-{1}", searchType, key);
            return searchKey;
        }
    }
}
