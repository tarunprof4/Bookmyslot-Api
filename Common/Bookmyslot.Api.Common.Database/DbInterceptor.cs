using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Common.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Database
{
    public class DbInterceptor : IDbInterceptor
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICompression compression;

        public DbInterceptor(IHttpContextAccessor httpContextAccessor, ICompression compression)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.compression = compression;
        }

        public async Task<T> GetQueryResults<T>(string sql, object parameters, Func<Task<T>> retrieveValues)
        {
          

            var requestId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.RequestId];
            var databaseRequestLog = new DatabaseRequestLog(requestId, sql, parameters);
            Log.Debug("{@databaseRequestLog}", databaseRequestLog);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = await retrieveValues.Invoke();
            stopWatch.Stop();

            var compresedResponseBody = compression.Compress(result);
            var databaseResponseLog = new DatabaseResponseLog(requestId, compresedResponseBody, stopWatch.Elapsed);
            Log.Debug("{@databaseResponseLog}", databaseResponseLog);

            return result;
        }
    
    }
}
