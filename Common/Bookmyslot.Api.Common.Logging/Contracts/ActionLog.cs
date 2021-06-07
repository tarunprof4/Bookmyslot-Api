namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class ActionLog
    { 
        public ActionLog(string operationName)
        {
            this.OperationName = operationName;
        }


        public string OperationName { get; set; }


    }
}
