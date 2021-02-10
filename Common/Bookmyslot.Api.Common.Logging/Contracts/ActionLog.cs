namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class ActionLog
    { 
        public ActionLog(string requestId, string operationName, string user)
        {
            this.RequestId = requestId;
            this.OperationName = operationName;
            this.User = user;
        }


        public string RequestId { get; set; }
        public string OperationName { get; set; }

        public string User { get; set; }

    }
}
