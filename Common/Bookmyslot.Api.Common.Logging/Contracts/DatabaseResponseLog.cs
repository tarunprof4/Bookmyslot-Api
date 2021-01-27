using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class DatabaseResponseLog 
    {
        public DatabaseResponseLog(string requestId,  string compressedResponse)
        {
            this.RequestId = requestId;
            this.CompressedResponse = compressedResponse;
        }

        public string RequestId { get; set; }
        public string CompressedResponse { get; set; }
    
    }
}
