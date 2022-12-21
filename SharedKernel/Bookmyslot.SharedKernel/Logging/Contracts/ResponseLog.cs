using System;

namespace Bookmyslot.SharedKernel.Logging.Contracts
{
    public class ResponseLog
    {
        public string CorrelationId { get; set; }

        public string RequestId { get; set; }

        public int StatusCode { get; set; }
        public string CompressedBody { get; set; }

        public TimeSpan ResponseTime { get; set; }
    }

}
