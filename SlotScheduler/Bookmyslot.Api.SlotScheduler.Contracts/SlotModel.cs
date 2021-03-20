using NodaTime;
using System;
using System.Text.Json.Serialization;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class SlotModel
    {
        public string Id { get; set; }
        
        public string Title { get; set; }

        public string CreatedBy { get; set; }
        
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

        public string SlotMeetingLink { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}
