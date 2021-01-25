using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class SqlDatabaseRequestLog
    {
        public SqlDatabaseRequestLog(IHttpContextAccessor httpContextAccessor, string sql, object parameters)
        {
            this.RequestId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.RequestId];
            this.LogTime = DateTime.UtcNow;
            this.Sql = sql;
            this.Parameters = parameters;
        }

        public string RequestId { get; set; }
        public DateTime LogTime { get; set; }
        public string Sql { get; set; }
        
        public object Parameters { get; set; }
    
    }
}
