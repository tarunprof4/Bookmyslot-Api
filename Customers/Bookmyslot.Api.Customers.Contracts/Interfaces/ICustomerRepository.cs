using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Response<CustomerModel>> GetCustomerByEmail(string email);

        Task<Response<CustomerModel>> GetCustomerById(string customerId);

        Task<Response<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds);
    }
}
