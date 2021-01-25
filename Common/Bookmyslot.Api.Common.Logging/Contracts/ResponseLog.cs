using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class ResponseLog
    {
        public string CorrelationId { get; set; }
        public string RequestId { get; set; }
        public DateTime LogTime { get; set; }

        public int StatusCode { get; set; }
        public string Body { get; set; }
        public string CompressedBody { get; set; }
    }

}
