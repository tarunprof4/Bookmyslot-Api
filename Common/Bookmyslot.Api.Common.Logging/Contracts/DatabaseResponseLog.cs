using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class DatabaseResponseLog
    {
        public DatabaseResponseLog(string coorelationId, string compressedResponse, TimeSpan responseTime)
        {
            this.CoorelationId = coorelationId;
            this.CompressedResponse = compressedResponse;
            this.ResponseTime = responseTime;
        }

        public string CoorelationId { get; set; }
        public string CompressedResponse { get; set; }

        public TimeSpan ResponseTime { get; set; }

    }
}
