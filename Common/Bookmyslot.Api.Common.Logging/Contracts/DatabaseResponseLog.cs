using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class DatabaseResponseLog 
    {
        public DatabaseResponseLog(string requestId,  string compressedResponse, TimeSpan responseTime)
        {
            this.RequestId = requestId;
            this.CompressedResponse = compressedResponse;
            this.ResponseTime = responseTime;
        }

        public string RequestId { get; set; }
        public string CompressedResponse { get; set; }

        public TimeSpan ResponseTime { get; set; }

    }
}
