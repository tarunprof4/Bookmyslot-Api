using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class EmailLog
    {
        public EmailLog(string requestId)
        {
            this.RequestId = requestId;
            this.LogTime = DateTime.UtcNow;
        }
        public string RequestId { get; set; }

        public DateTime LogTime { get; set; }

    }

}
