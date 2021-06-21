using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Search.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Constants;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ElasticClient elasticClient;
        private readonly IDbInterceptor dbInterceptor;
        private readonly ILoggerService loggerService;
        public CustomerRepository(ElasticClient elasticClient, IDbInterceptor dbInterceptor, ILoggerService loggerService)
        {
            this.elasticClient = elasticClient;
            this.dbInterceptor = dbInterceptor;
            this.loggerService = loggerService;
        }
        public async Task<Response<bool>> CreateCustomer(SearchCustomerModel searchCustomerModel)
        {
            return await CreateUpdateCustomer(searchCustomerModel, "CreateCustomer", AppBusinessMessagesConstants.CreateCustomerFailed);
        }

        public async Task<Response<bool>> UpdateCustomer(SearchCustomerModel searchCustomerModel)
        {
            return await CreateUpdateCustomer(searchCustomerModel, "UpdateCustomer", AppBusinessMessagesConstants.UpdateCustomerFailed);
        }

        private async Task<Response<bool>> CreateUpdateCustomer(SearchCustomerModel searchCustomerModel, string operationName, string operationFailedMessage)
        {
            var createCustomerResponse = await this.dbInterceptor.GetQueryResults(operationName, searchCustomerModel, () => this.elasticClient.IndexDocumentAsync(searchCustomerModel));
            if (createCustomerResponse.IsValid)
            {
                return new Response<bool>() { Result = true };
            }

            this.loggerService.Error(createCustomerResponse.OriginalException, string.Empty);
            return Response<bool>.Error(new List<string>() { operationFailedMessage });
        }
    }
}
