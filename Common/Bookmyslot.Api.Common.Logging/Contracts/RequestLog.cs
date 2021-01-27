using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class RequestLog
    {
        public string CorrelationId { get; set; }

        public string RequestId { get; set; }

        public DateTime LogTime { get; set; }

        public string Schema { get; set; }

        public HostString Host { get; set; }


        public PathString Path { get; set; }

        public string Method { get; set; }

        public QueryString QueryString { get; set; }

        public string Body { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }

}
