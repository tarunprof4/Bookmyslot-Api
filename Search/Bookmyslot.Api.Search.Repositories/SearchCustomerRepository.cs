
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Customers.Repositories.ModelFactory;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Constants.cs;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Repositories
{
    public class SearchCustomerRepository : ISearchCustomerRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;
        private readonly ICompression compression;

        public SearchCustomerRepository(IDbConnection connection, IDbInterceptor dbInterceptor, ICompression compression)
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
            var sql = SearchCustomerTableQueries.InsertOrUpdatePreProcessedSearchedCustomerQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SavePreProcessedSearchedCustomers", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Response<bool>() { Result = true };

        }

        public async Task<Response<List<SearchCustomerModel>>> GetPreProcessedSearchedCustomers(string searchKey)
        {
            var parameters = new { SearchKey = CreateSearchKey(searchKey) };
            var sql = SearchCustomerTableQueries.GetPreProcessedSearchedCustomerQuery;

            var compressedSearchedCustomers = await this.dbInterceptor.GetQueryResults("GetPreProcessedSearchedCustomers", parameters, () => this.connection.QueryFirstOrDefaultAsync<string>(sql, parameters));

            var deCompressedSearchCustomerModels = this.compression.Decompress<List<SearchCustomerModel>>(compressedSearchedCustomers);
            if(deCompressedSearchCustomerModels.Count == 0)
            {
                return Response<List<SearchCustomerModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoCustomerSearchResults });
            }
            return new Response<List<SearchCustomerModel>>() { Result = deCompressedSearchCustomerModels };
        }



        public async Task<Response<List<SearchCustomerModel>>> SearchCustomers(string searchKey)
        {
            var parameters = new { SearchKey = searchKey };
            var sql = SearchCustomerTableQueries.SearchCustomerQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SearchCustomers", parameters, () => this.connection.QueryAsync<SearchCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateSearchCustomerModelsResponse(searchCustomerEntities);
        }

        private string CreateSearchKey(string key)
        {
            var searchKey = string.Format("{0}-{1}", SearchConstants.SearchCustomer, key);
            return searchKey;
        }

    }
}
