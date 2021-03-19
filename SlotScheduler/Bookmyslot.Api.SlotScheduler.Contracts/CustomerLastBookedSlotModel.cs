using NodaTime;
using System;
using System.Text.Json.Serialization;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class CustomerLastBookedSlotModel
    {
        public string CustomerId { get; set; }

        public string Title { get; set; }
        public string Country { get; set; }

        public ZonedDateTime SlotZonedDate { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotStartTime { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotEndTime { get; set; }

        
    }
}
