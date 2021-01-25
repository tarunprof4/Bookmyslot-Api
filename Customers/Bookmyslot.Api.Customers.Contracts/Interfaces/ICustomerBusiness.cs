using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerBusiness
    {
        Task<Response<string>> CreateCustomer(CustomerModel customerModel);
        Task<Response<IEnumerable<CustomerModel>>> GetAllCustomers();

        Task<Response<CustomerModel>> GetCustomerByEmail(string email);

        Task<Response<CustomerModel>> GetCustomerById(string customerId);


        Task<Response<bool>> UpdateCustomer(CustomerModel customerModel);

        Task<Response<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds);
    }
}
