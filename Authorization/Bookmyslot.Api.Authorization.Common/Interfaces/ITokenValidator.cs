using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authorization.Common.Interfaces
{
    public interface ITokenValidator
    {
        Task<Response<SocialCustomer>> ValidateToken(string token);
    }
}
