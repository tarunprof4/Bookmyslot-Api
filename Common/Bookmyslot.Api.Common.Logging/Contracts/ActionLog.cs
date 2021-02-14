namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class ActionLog
    { 
        public ActionLog(string operationName, string user)
        {
            this.OperationName = operationName;
            this.User = user;
        }


        public string OperationName { get; set; }

        public string User { get; set; }

    }
}
