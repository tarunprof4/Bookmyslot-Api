using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface IGoogleTokenValidator
    {
        Task<Result<SocialCustomerModel>> ValidateToken(string idToken);
    }
}
