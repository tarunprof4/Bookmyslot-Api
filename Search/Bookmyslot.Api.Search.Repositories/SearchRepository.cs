using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Constants.cs;
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


        public async Task<Response<bool>> SavePreProcessedSearchedCustomers(string searchKey, List<SearchCustomerModel> searchCustomerModels)
        {
            var parameters = new
            {
                SearchKey = CreateSearchKey(searchKey),
                Value = this.compression.Compress(searchCustomerModels),
                ModifiedDateUtc = DateTime.UtcNow
            };
            var sql = SearchQueries.InsertOrUpdatePreProcessedSearchedCustomerQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SavePreProcessedSearchedCustomers", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Response<bool>() { Result = true };

        }

        public async Task<Response<List<SearchCustomerModel>>> GetPreProcessedSearchedCustomers(string searchKey)
        {
            var parameters = new { SearchKey = CreateSearchKey(searchKey) };
            var sql = SearchQueries.GetPreProcessedSearchedCustomerQuery;

            var compressedSearchedCustomers = await this.dbInterceptor.GetQueryResults("GetPreProcessedSearchedCustomers", parameters, () => this.connection.QueryFirstOrDefaultAsync<SearchEntity>(sql, parameters));

            if (compressedSearchedCustomers != null)
            {
                var deCompressedSearchCustomerModels = this.compression.Decompress<List<SearchCustomerModel>>(compressedSearchedCustomers.Value);
                return new Response<List<SearchCustomerModel>>() { Result = deCompressedSearchCustomerModels };
            }

            return Response<List<SearchCustomerModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoCustomerSearchResults });

        }

        private string CreateSearchKey(string key)
        {
            var searchKey = string.Format("{0}-{1}", SearchConstants.SearchCustomer, key);
            return searchKey;
        }

    }
}
