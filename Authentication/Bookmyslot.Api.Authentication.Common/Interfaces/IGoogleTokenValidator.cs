using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface IGoogleTokenValidator
    {
        Task<Response<SocialCustomerModel>> ValidateToken(string idToken);
    }
}
