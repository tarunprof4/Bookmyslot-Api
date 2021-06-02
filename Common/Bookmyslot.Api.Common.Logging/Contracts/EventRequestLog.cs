namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class EventRequestLog
    {
        public EventRequestLog(string coorelationId, string eventName, object parameters)
        {
            this.CoorelationId = coorelationId;
            this.EventName = eventName;
            this.Parameters = parameters;
        }

        public string CoorelationId { get; set; }
        public string EventName { get; set; }

        public object Parameters { get; set; }
    }
}
