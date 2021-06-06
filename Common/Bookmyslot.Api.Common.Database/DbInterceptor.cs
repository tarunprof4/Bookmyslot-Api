using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Compression;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Logging.Constants;
using Bookmyslot.Api.Common.Logging.Contracts;
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
            var coorelationId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.CoorelationId];
            var databaseRequestLog = new DatabaseRequestLog(coorelationId, operationName, parameters);
            this.loggerService.Debug("{@databaseRequestLog}", databaseRequestLog);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = await retrieveValues.Invoke();
            stopWatch.Stop();

            var compresedResponseBody = compression.Compress(result);
            var databaseResponseLog = new DatabaseResponseLog(coorelationId, compresedResponseBody, stopWatch.Elapsed);
            this.loggerService.Debug("{@databaseResponseLog}", databaseResponseLog);

            return result;
        }
    
    }
}
