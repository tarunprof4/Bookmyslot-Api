using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Customers.Repositories.ModelFactory;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Bookmyslot.Api.Search.Repositories.Queries;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        public async Task<Response<SearchCustomerModel>> SearchCustomersByUserName(string userName)
        {
            var parameters = new { userName = userName.ToLowerInvariant() };
            var sql = RegisterCustomerTableQueries.SearchCustomerByUserNameQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SearchCustomersByUserName", parameters, () => this.connection.QueryFirstOrDefaultAsync<SearchCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateSearchCustomerModelResponse(searchCustomerEntities);
        }


        public async Task<Response<List<SearchCustomerModel>>> SearchCustomersByName(string name)
        {
            var searchName = GenerateSearchByNameKey(name).ToString();
            var parameters = new { name = searchName };
            var sql = RegisterCustomerTableQueries.SearchCustomerByNameQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SearchCustomersByName", parameters, () => this.connection.QueryAsync<SearchCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateSearchCustomerModelsResponse(searchCustomerEntities);
        }

        public async Task<Response<List<SearchCustomerModel>>> SearchCustomersByBioHeadLine(string bioHeadLine)
        {
            var parameters = new { bioHeadLine = bioHeadLine };
            var sql = RegisterCustomerTableQueries.SearchCustomerByBioHeadLineQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SearchCustomersByBioHeadLine", parameters, () => this.connection.QueryAsync<SearchCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateSearchCustomerModelsResponse(searchCustomerEntities);
        }

     
        private StringBuilder GenerateSearchByNameKey(string name)
        {
            StringBuilder searchName = new StringBuilder();
            var splitNameArray = name.Split().Distinct();
            foreach(var splitName in splitNameArray)
            {
                searchName.Append(splitName);
                searchName.Append("* ");
            }

            return searchName;
        }
      
    }
}
