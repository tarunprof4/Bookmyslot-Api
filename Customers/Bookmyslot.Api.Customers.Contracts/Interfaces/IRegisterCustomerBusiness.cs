using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IRegisterCustomerBusiness
    {
        Task<Response<string>> RegisterCustomer(RegisterCustomerModel registerCustomerModel);
    }
}
