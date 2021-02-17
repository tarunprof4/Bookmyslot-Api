
using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface ISocialLoginTokenValidator
    {
        Task<Response<SocialCustomerModel>> ValidateToken(string token);
    }
}
