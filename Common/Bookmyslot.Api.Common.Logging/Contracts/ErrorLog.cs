using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class ErrorLog
    {
        public string RequestId { get; set; }
        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
