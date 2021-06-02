using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Logging.Contracts
{
    public class EventCompleteLog
    {
        public EventCompleteLog(string coorelationId, TimeSpan responseTime)
        {
            this.CoorelationId = coorelationId;
            this.ResponseTime = responseTime;
        }

        public string CoorelationId { get; set; }

        public object ResponseTime { get; set; }
    }
