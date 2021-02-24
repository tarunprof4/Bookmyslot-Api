using Bookmyslot.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Common.Interfaces
{
    public interface ICurrentUser
    {
        Task<Response<string>> GetCurrentUserFromCache(string email);

        Task SetCurrentUserInCache(string email);

        string GetCurrentUserEmail();
    }
}
