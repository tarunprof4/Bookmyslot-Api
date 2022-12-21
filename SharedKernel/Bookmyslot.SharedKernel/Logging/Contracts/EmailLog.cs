namespace Bookmyslot.SharedKernel.Logging.Contracts
{
    public class EmailLog
    {
        public EmailLog(string coorelationId)
        {
            this.CoorelationId = coorelationId;
        }
        public string CoorelationId { get; set; }


    }

}
