using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository;
using Nest;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ElasticClient elasticClient;
        private readonly IDbInterceptor dbInterceptor;
        public CustomerRepository(ElasticClient elasticClient, IDbInterceptor dbInterceptor)
        {
            this.elasticClient = elasticClient;
            this.dbInterceptor = dbInterceptor;
        }
        public async Task<Response<bool>> CreateCustomer(CustomerModel customerModel)
        {
            var createCustomerResponse = await this.dbInterceptor.GetQueryResults("CreateCustomer", customerModel, () => this.elasticClient.IndexDocumentAsync(customerModel));
            if (createCustomerResponse.Result == Result.Created)
            {
                
                return new Response<bool>() { Result = true };
            }

            
            return new Response<bool>() { Result = false };
        }
    }
}
