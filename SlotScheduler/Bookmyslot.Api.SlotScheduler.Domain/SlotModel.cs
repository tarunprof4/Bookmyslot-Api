using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Domain.DomainEvents;
using Bookmyslot.Api.SlotScheduler.Domain.Events;
using NodaTime;
using System;

namespace Bookmyslot.Api.SlotScheduler.Domain
{
    public class SlotModel : BaseEventModel
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

        public bool IsSlotDateValid()
        {
            var utcZoneTime = SystemClock.Instance.GetCurrentInstant().InUtc();
            Duration timeDifference = this.SlotStartZonedDateTime - utcZoneTime;
            if (timeDifference.TotalHours > 0)
            {
                return true;
            }
            return false;
        }

        public bool SlotNotAllowedOnDayLightSavingDay()
        {
            var isDayLightSavingDay = this.SlotStartZonedDateTime.IsDaylightSavingTime();
            if (isDayLightSavingDay)
            {
                return false;
            }
            return true;
        }


        public bool IsSlotEndTimeValid()
        {
            if (this.SlotEndTime > this.SlotStartTime)
            {
                return true;
            }
            return false;
        }


        public bool IsSlotDurationValid()
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


        public bool IsSlotBookedByHimself(string bookedBy)
        {
            if (bookedBy == this.CreatedBy)
            {
                return true;
            }

            return false;
        }



        public bool IsSlotScheduleDateValid()
        {
            var pendingTimeForSlotMeeting = this.SlotStartZonedDateTime - NodaTimeHelper.GetCurrentUtcZonedDateTime();
            if (pendingTimeForSlotMeeting.Milliseconds < 0)
            {
                return false;
            }
            return true;
        }


        public void ScheduleSlot(CustomerSummaryModel bookedByCustomerSummaryModel)
        {
            Events.Add(new SlotScheduledDomainEvent(this, bookedByCustomerSummaryModel));
            this.BookedBy = bookedByCustomerSummaryModel.Id;
            this.SlotMeetingLink = CreateSlotMeetingUrl();
        }

        private string CreateSlotMeetingUrl()
        {
            var uniqueId = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var meetingLinkUrl = string.Format("{0}/{1}", SlotConstants.JitsiUrl, uniqueId);

            return meetingLinkUrl;
        }

        public void ResendSlotMeetingInformation(string resendTo)
        {
            Events.Add(new SlotMeetingInformationRequestedEvent(this.Id, resendTo));
        }


    }
}
