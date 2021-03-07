using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ILoginCustomerBusiness
    {
        Task<Response<string>> LoginGoogleCustomer(SocialCustomerModel socialCustomerModel);

        Task<Response<string>> LoginFacebokCustomer(SocialCustomerModel socialCustomerModel);
    }
}
