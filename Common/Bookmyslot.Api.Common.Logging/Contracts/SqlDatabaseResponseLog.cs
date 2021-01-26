using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class SqlDatabaseResponseLog 
    {
        public SqlDatabaseResponseLog(IHttpContextAccessor httpContextAccessor,  string compressedResponse)
        {
            this.RequestId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.RequestId];
            this.LogTime = DateTime.UtcNow;
            this.CompressedResponse = compressedResponse;
        }

        public string RequestId { get; set; }
        public DateTime LogTime { get; set; }
        
        public string CompressedResponse { get; set; }
    
    }
}
