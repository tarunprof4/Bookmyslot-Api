using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface ISocialLoginTokenValidator
    {
        Task<Result<SocialCustomerModel>> LoginWithGoogle(string idToken);

        Task<Result<SocialCustomerModel>> LoginWithFacebook(string authToken);
    }
}
