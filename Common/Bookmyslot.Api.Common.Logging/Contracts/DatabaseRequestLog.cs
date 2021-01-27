using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class DatabaseRequestLog
    { 
        public DatabaseRequestLog(string requestId, string operationName, object parameters)
        {
            this.RequestId = requestId;
            this.OperationName = operationName;
            this.Parameters = parameters;
        }

        

        public string RequestId { get; set; }
        public string OperationName { get; set; }
        
        public object Parameters { get; set; }
    
    }
}
