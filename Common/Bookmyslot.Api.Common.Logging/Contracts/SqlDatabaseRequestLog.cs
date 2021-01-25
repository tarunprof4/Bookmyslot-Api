using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class SqlDatabaseRequestLog
    { 
        public SqlDatabaseRequestLog(IHttpContextAccessor httpContextAccessor, string operationName, object parameters)
        {
            this.RequestId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.RequestId];
            this.LogTime = DateTime.UtcNow;
            this.OperationName = operationName;
            this.Parameters = parameters;
        }

        

        public string RequestId { get; set; }
        public DateTime LogTime { get; set; }
        public string OperationName { get; set; }
        
        public object Parameters { get; set; }
    
    }
}
