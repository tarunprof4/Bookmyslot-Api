using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Database
{
    public class SqlInterceptor : ISqlInterceptor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SqlInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<T> GetQueryResults<T>(string sql, object parameters, Func<Task<T>> retrieveValues)
        {
            var databaseRequestLog = new SqlDatabaseRequestLog(this.httpContextAccessor, sql, parameters);
            Log.Debug("{@databaseRequestLog}", databaseRequestLog);

            var result = await retrieveValues.Invoke();

            var databaseResponsetLog = new SqlDatabaseResponseLog(this.httpContextAccessor, result);
            Log.Debug("{@databaseResponseLog}", databaseResponsetLog);

            return result;
        }
    
    }
}
