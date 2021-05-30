using Bookmyslot.Api.SlotScheduler.Contracts.Constants;
using NodaTime;
using System;

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

        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }

        public string SlotMeetingLink { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public bool isSlotDateValid()
        {
            var utcZoneTime = SystemClock.Instance.GetCurrentInstant().InUtc();
            Duration timeDifference = this.SlotStartZonedDateTime - utcZoneTime;
            if (timeDifference.TotalHours > 0)
            {
                return true;
            }
            return false;
        }

        public bool slotNotAllowedOnDayLightSavingDay()
        {
            var isDayLightSavingDay = this.SlotStartZonedDateTime.IsDaylightSavingTime();
            if (isDayLightSavingDay)
            {
                return false;
            }
            return true;
        }


        public bool isSlotEndTimeValid()
        {
            if (this.SlotEndTime > this.SlotStartTime)
            {
                return true;
            }
            return false;
        }


        public bool isSlotDurationValid()
        {
            if (this.SlotDuration.TotalMinutes >= SlotConstants.MinimumSlotDuration)
            {
                return true;
            }
            return false;
        }

        public string CancelSlot(string cancelledBy)
        {
            if (cancelledBy == this.BookedBy)
            {
                this.BookedBy = string.Empty;
                this.SlotMeetingLink = string.Empty;
                return SlotConstants.UpdateSlot;
            }

            return SlotConstants.DeleteSlot;
        }



    }
}
