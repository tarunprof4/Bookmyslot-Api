using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface ICurrentUser
    {
        Task<Response<CurrentUserModel>> GetCurrentUserFromCache();

        Task SetCurrentUserInCache(string email);
    }
}
