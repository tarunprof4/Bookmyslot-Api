
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database
{
    public interface IDbInterceptor
    {
        Task<T> GetQueryResults<T>(string operationName, object parameters, Func<Task<T>> retrieveValues);
    }

}
