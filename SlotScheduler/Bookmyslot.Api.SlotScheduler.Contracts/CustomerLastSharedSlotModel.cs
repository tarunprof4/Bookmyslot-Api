﻿using NodaTime;
using System;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class CustomerLastSharedSlotModel
    {
        public string CreatedBy { get; set; }

        public string Title { get; set; }
        public string Country { get; set; }

        public string TimeZone { get; set; }

        public ZonedDateTime SlotStartZonedDateTime { get; set; }

        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }


    }
}
