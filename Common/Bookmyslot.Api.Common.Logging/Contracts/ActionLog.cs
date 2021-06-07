namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class ActionLog
    { 
        public ActionLog(string operationName, string userName)
        {
            this.OperationName = operationName;
            this.UserName = userName;
        }


        public string OperationName { get; set; }

        public string UserName { get; set; }

    }
}
