using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
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

        public async Task<Response<T>> GetPreProcessedSearchedResponse<T>(string searchType, string searchKey)
        {
            
            var parameters = new { SearchKey = CreateSearchKey(searchType, searchKey) };
            var sql = SearchQueries.GetPreProcessedSearchedCustomerQuery;

            var compressedSearchedCustomers = await this.dbInterceptor.GetQueryResults("GetPreProcessedSearchedCustomers", parameters, () => this.connection.QueryFirstOrDefaultAsync<SearchEntity>(sql, parameters));

            if (compressedSearchedCustomers != null)
            {
                var deCompressedSearchCustomerModels = this.compression.Decompress<T>(compressedSearchedCustomers.Value);
                return new Response<T>() { Result = deCompressedSearchCustomerModels };
            }

            return Response<T>.Empty(new List<string>() { AppBusinessMessagesConstants.NoCustomerSearchResults });
        }

        public async Task<Response<bool>> SavePreProcessedSearchedResponse<T>(string searchType, string searchKey, T response)
        {
            var parameters = new
            {
                SearchKey = CreateSearchKey(searchType, searchKey),
                Value = this.compression.Compress(response),
                ModifiedDateUtc = DateTime.UtcNow
            };
            var sql = SearchQueries.InsertOrUpdatePreProcessedSearchedCustomerQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SavePreProcessedSearchedCustomers", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Response<bool>() { Result = true };
        }

     

        private string CreateSearchKey(string searchType, string key)
        {
            var searchKey = string.Format("{0}-{1}", searchType, key);
            return searchKey;
        }
    }
}
