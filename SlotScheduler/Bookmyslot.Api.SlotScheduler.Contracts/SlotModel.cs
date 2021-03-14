﻿
using NodaTime;
using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class SlotModel
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }

        public string CreatedBy { get; set; }
        
        public string BookedBy { get; set; }

        public ZonedDateTime SlotZonedDate { get; set; }

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

        public DateTime CreatedDateUtc { get; set; }
    }
}
