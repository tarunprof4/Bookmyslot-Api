using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Common.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Bookmyslot.Api.Common.Logging.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Database
{
    public class DbInterceptor : IDbInterceptor
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICompression compression;
        private readonly ILoggerService loggerService;

        public DbInterceptor(IHttpContextAccessor httpContextAccessor, ICompression compression, ILoggerService loggerService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.compression = compression;
            this.loggerService = loggerService;
        }

        public async Task<T> GetQueryResults<T>(string operationName, object parameters, Func<Task<T>> retrieveValues)
        {
            var requestId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.RequestId];
            var databaseRequestLog = new DatabaseRequestLog(requestId, operationName, parameters);
            this.loggerService.Debug("{@databaseRequestLog}", databaseRequestLog);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = await retrieveValues.Invoke();
            stopWatch.Stop();

            var compresedResponseBody = compression.Compress(result);
            var databaseResponseLog = new DatabaseResponseLog(requestId, compresedResponseBody, stopWatch.Elapsed);
            this.loggerService.Debug("{@databaseResponseLog}", databaseResponseLog);

            return result;
        }
    
    }
}
