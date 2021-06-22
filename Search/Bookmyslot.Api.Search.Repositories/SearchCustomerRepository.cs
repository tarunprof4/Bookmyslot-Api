using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Customers.Repositories.ModelFactory;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Search.Repositories.Enitites;
using Bookmyslot.Api.Search.Repositories.Queries;
using Bookmyslot.Bookmyslot.Api.Common.Search.Constants;
using Dapper;
using Nest;
using System.Collections.Generic;
using System.Data;
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
                    Includes = this.GetIncludedFields().ToArray(),
                }
            };

            var response = await this.dbInterceptor.GetQueryResults("SearchCustomersByName", name, () =>
            this.elasticClient.SearchAsync<SearchCustomerModel>(request));

            return ResponseModelFactory.CreateSearchCustomerModelsResponse(response);
        }

        

        public async Task<Response<List<SearchCustomerModel>>> SearchCustomersByBioHeadLine(string bioHeadLine, PageParameterModel pageParameterModel)
        {
            var request = new SearchRequest<SearchCustomerModel>()
            {
                From = pageParameterModel.PageNumber,
                Size = pageParameterModel.PageSize,
                Query = new MatchQuery
                {
                    Field = Infer.Field<SearchCustomerModel>(f => f.BioHeadLine),
                    Query = bioHeadLine,
                    Analyzer = ElasticSearchConstants.StandardAnalyzer,
                    Lenient = true,
                },
                Source = new SourceFilter
                {
                    Includes = this.GetIncludedFields().ToArray(),
                }
            };

            var response = await this.dbInterceptor.GetQueryResults("SearchCustomersByBioHeadLine", bioHeadLine, () =>
            this.elasticClient.SearchAsync<SearchCustomerModel>(request));

            return ResponseModelFactory.CreateSearchCustomerModelsResponse(response);
        }

        private List<Field> GetIncludedFields()
        {
            return new List<Field>
            {
                Infer.Field<SearchCustomerModel>(f => f.UserName),
                Infer.Field<SearchCustomerModel>(f => f.FirstName),
                Infer.Field<SearchCustomerModel>(f => f.LastName),
                Infer.Field<SearchCustomerModel>(f => f.ProfilePictureUrl)
            };
        }
    }
}
