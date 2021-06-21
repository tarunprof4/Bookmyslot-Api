using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Common.Search.Contracts;
using Bookmyslot.Api.Customers.Repositories.ModelFactory;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Bookmyslot.Api.Search.Repositories.Queries;
using Dapper;
using Nest;
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
        private readonly ElasticClient elasticClient;

        public SearchCustomerRepository(IDbConnection connection, IDbInterceptor dbInterceptor, ElasticClient elasticClient)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
            this.elasticClient = elasticClient;
        }

        public async Task<Response<SearchCustomerModel>> SearchCustomersByUserName(string userName)
        {
            var parameters = new { userName = userName.ToLowerInvariant() };
            var sql = RegisterCustomerTableQueries.SearchCustomerByUserNameQuery;

            var searchCustomerEntities = await this.dbInterceptor.GetQueryResults("SearchCustomersByUserName", parameters, () => this.connection.QueryFirstOrDefaultAsync<SearchCustomerEntity>(sql, parameters));

            return ResponseModelFactory.CreateSearchCustomerModelResponse(searchCustomerEntities);
        }


        public async Task<Response<List<SearchCustomerModel>>> SearchCustomersByName(string name, PageParameterModel pageParameterModel)
        {
            var includeFields = new List<Field>();
            includeFields.Add(Infer.Field<SearchCustomerModel>(f => f.Id));
            includeFields.Add(Infer.Field<SearchCustomerModel>(f => f.FullName));

            var firstNameField = Infer.Field<SearchCustomerModel>(f => f.FirstName);
            var lastNameField = Infer.Field<SearchCustomerModel>(f => f.LastName);
            var fullNameField = Infer.Field<SearchCustomerModel>(f => f.FullName);
            Field[] multiMatchFields = { firstNameField, lastNameField, fullNameField };

            var request = new SearchRequest<SearchCustomerModel>()
            {
                From = pageParameterModel.PageNumber,
                Size = pageParameterModel.PageSize,
                Query = new MultiMatchQuery
                {
                    Fields = multiMatchFields,
                    Query = name,
                    Type = TextQueryType.BoolPrefix,
                },
                Source = new SourceFilter
                {
                    Includes = includeFields.ToArray(),
                }
            };

            var response = await this.dbInterceptor.GetQueryResults("SearchCustomersByName", name, () =>
            this.elasticClient.SearchAsync<SearchCustomerModel>(request));

            if(response.Documents.Count == 0)
            {
                return Response<List<SearchCustomerModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Response<List<SearchCustomerModel>>() { Result= response.Documents.ToList() } ;
         
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
