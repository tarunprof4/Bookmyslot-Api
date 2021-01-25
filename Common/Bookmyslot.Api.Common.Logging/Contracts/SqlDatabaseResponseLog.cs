using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class SqlDatabaseResponseLog 
    {
        public SqlDatabaseResponseLog(IHttpContextAccessor httpContextAccessor,  object response)
        {
            this.RequestId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.RequestId];
            this.LogTime = DateTime.UtcNow;
            this.Response = response;
        }

        public string RequestId { get; set; }
        public DateTime LogTime { get; set; }
        
        public object Response { get; set; }
    
    }
}
