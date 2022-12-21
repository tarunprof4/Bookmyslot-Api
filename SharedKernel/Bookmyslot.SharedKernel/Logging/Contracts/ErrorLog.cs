using System;

namespace Bookmyslot.SharedKernel.Logging.Contracts
{
    public class ErrorLog
    {
        public string RequestId { get; set; }
        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
