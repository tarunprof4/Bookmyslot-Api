using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface ICurrentUser
    {
        Task<Result<CurrentUserModel>> GetCurrentUserFromCache();

        Task SetCurrentUserInCache(string email);
    }
}
