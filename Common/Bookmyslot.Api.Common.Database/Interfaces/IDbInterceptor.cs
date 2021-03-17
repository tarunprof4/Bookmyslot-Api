
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Database.Interfaces
{
    public interface IDbInterceptor
    {
        Task<T> GetQueryResults<T>(string operationName, object parameters, Func<Task<T>> retrieveValues);
    }

}
