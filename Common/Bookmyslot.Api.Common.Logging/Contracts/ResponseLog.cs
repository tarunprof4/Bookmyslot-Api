using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class ResponseLog
    {
        public string CorrelationId { get; set; }

        public string RequestId { get; set; }

        public int StatusCode { get; set; }
        public string CompressedBody { get; set; }
    }

}
