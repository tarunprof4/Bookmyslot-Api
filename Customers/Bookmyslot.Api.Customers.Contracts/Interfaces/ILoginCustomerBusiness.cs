using Bookmyslot.Api.Authorization.Common;
using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ILoginCustomerBusiness
    {
        Task<Response<string>> LoginSocialCustomer(SocialCustomer socialCustomer);
    }
}
