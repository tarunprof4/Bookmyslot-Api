using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Common.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Database
{
    public class SqlInterceptor : IDbInterceptor
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICompression compression;

        public SqlInterceptor(IHttpContextAccessor httpContextAccessor, ICompression compression)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.compression = compression;
        }

        public async Task<T> GetQueryResults<T>(string sql, object parameters, Func<Task<T>> retrieveValues)
        {
            var requestId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.RequestId];
            var databaseRequestLog = new SqlDatabaseRequestLog(requestId, sql, parameters);
            Log.Debug("{@databaseRequestLog}", databaseRequestLog);

            var result = await retrieveValues.Invoke();

            var compresedResponseBody = compression.Compress(result);

            var databaseResponsetLog = new SqlDatabaseResponseLog(requestId, compresedResponseBody);
            Log.Debug("{@databaseResponseLog}", databaseResponsetLog);

            return result;
        }
    
    }
}
