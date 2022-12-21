using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Result<string>> GetCustomerIdByEmail(string email);
        Task<Result<CustomerModel>> GetCustomerByEmail(string email);

        Task<Result<CustomerModel>> GetCustomerById(string customerId);

        Task<Result<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds);
    }
}
