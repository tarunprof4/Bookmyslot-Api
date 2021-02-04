namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class EmailLog
    {
        public EmailLog(string requestId)
        {
            this.RequestId = requestId;
        }
        public string RequestId { get; set; }


    }

}
