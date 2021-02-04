
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Customers.Repositories.ModelFactory;
using Bookmyslot.Api.Search.Contracts;
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
    public class SearchCustomerRepository : ISearchCustomerRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public SearchCustomerRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }
        public async Task<Response<List<SearchCustomerModel>>> SearchCustomers(string searchKey)
        {
            var parameters = new { searchKey = searchKey };
            var sql = SearchCustomerTableQueries.SearchCustomerQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SearchCustomers", parameters, () => this.connection.QueryAsync<SearchCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateSearchCustomerModelsResponse(searchCustomerEntities);
        }
    }
}
