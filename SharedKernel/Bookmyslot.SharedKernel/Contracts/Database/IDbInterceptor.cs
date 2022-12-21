using System;
using System.Threading.Tasks;

namespace Bookmyslot.SharedKernel.Contracts.Database
{
    public interface IDbInterceptor
    {
        Task<T> GetQueryResults<T>(string operationName, object parameters, Func<Task<T>> retrieveValues);
    }

}
