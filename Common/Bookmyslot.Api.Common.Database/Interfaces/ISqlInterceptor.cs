
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Database.Interfaces
{
    public interface ISqlInterceptor
    {
        Task<T> GetQueryResults<T>(string sql, object parameters, Func<Task<T>> retrieveValues);
    }

}
