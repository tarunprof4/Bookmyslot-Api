using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ILoginCustomerBusiness
    {
        Task<Result<string>> LoginSocialCustomer(SocialCustomerLoginModel socialCustomerLoginModel);
    }
}
