using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository;
using Bookmyslot.SharedKernel.ValueObject;
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
        public async Task<Result<bool>> CreateCustomer(CustomerModel customerModel)
        {
            return await this.customerRepository.CreateCustomer(customerModel);
        }

        public async Task<Result<bool>> UpdateCustomer(CustomerModel customerModel)
        {
            return await this.customerRepository.UpdateCustomer(customerModel);
        }
    }
}
