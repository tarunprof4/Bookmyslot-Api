using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface ITokenValidator
    {
        Task<Response<SocialCustomerModel>> ValidateToken(string token);
    }
}
