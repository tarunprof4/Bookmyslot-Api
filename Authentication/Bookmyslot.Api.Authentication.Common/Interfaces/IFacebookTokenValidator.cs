using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{

    public interface IFacebookTokenValidator
    {
        Task<Result<SocialCustomerModel>> ValidateAccessToken(string authToken);
    }
}
