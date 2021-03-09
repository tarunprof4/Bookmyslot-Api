using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{

    public interface IFacebookTokenValidator
    {
        Task<Response<SocialCustomerModel>> ValidateAccessToken(string authToken);
    }
}
