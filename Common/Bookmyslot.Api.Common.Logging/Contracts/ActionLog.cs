namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class ActionLog
    { 
        public ActionLog(string operationName, string customerId)
        {
            this.OperationName = operationName;
            this.CustomerId = customerId;
        }


        public string OperationName { get; set; }

        public string CustomerId { get; set; }

    }
}
