using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Search.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        public async Task<Response<bool>> CreateCustomer(SearchCustomerModel searchCustomerModel)
        {
            return await this.customerRepository.CreateCustomer(searchCustomerModel);
        }

        public async Task<Response<bool>> UpdateCustomer(SearchCustomerModel searchCustomerModel)
        {
            return await this.customerRepository.UpdateCustomer(searchCustomerModel);
        }
    }
}
