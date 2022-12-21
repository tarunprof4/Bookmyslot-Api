using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Constants;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository;
using Bookmyslot.SharedKernel.Contracts.Database;
using Bookmyslot.SharedKernel.Contracts.Logging;
using Bookmyslot.SharedKernel.ValueObject;
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
        public async Task<Result<bool>> CreateCustomer(CustomerModel customerModel)
        {
            return await CreateUpdateCustomer(customerModel, "CreateCustomer", AppBusinessMessagesConstants.CreateCustomerFailed);
        }

        public async Task<Result<bool>> UpdateCustomer(CustomerModel customerModel)
        {
            return await CreateUpdateCustomer(customerModel, "UpdateCustomer", AppBusinessMessagesConstants.UpdateCustomerFailed);
        }

        private async Task<Result<bool>> CreateUpdateCustomer(CustomerModel customerModel, string operationName, string operationFailedMessage)
        {
            var createCustomerResponse = await this.dbInterceptor.GetQueryResults(operationName, customerModel, () => this.elasticClient.IndexDocumentAsync(customerModel));
            if (createCustomerResponse.IsValid)
            {
                return new Result<bool>() { Value = true };
            }

            this.loggerService.Error(createCustomerResponse.OriginalException, string.Empty);
            return Result<bool>.Error(new List<string>() { operationFailedMessage });
        }
    }
}
