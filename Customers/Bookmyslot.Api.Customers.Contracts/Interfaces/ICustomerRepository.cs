using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Response<string>> GetCustomerIdByEmail(string email);
        Task<Response<CustomerModel>> GetCustomerByEmail(string email);

        Task<Response<CustomerModel>> GetCustomerById(string customerId);

        Task<Response<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds);
    }
}
