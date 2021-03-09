
using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface ISocialLoginTokenValidator
    {
        Task<Response<SocialCustomerModel>> LoginWithGoogle(string token);

        Task<Response<SocialCustomerModel>> LoginWithFacebook(string authToken);
    }
}
