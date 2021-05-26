namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class DatabaseRequestLog
    { 
        public DatabaseRequestLog(string coorelationId, string operationName, object parameters)
        {
            this.CoorelationId = coorelationId;
            this.OperationName = operationName;
            this.Parameters = parameters;
        }

        

        public string CoorelationId { get; set; }
        public string OperationName { get; set; }
        
        public object Parameters { get; set; }
    
    }
}
