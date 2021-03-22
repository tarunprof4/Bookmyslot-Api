using NodaTime;
using System;
using System.Text.Json.Serialization;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class CancelledSlotModel
    {
        [JsonIgnore]
        public string Id { get; set; }
        
        public string Title { get; set; }

        [JsonIgnore]
        public string CreatedBy { get; set; }

        [JsonIgnore]
        public string CancelledBy { get; set; }

        [JsonIgnore]
        public string BookedBy { get; set; }

        public string Country { get; set; }
        public ZonedDateTime SlotStartZonedDateTime { get; set; }


        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotStartTime { get; set; }


        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotEndTime { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }

    }
}
