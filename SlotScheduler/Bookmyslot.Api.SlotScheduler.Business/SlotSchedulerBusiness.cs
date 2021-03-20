using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class SlotSchedulerBusiness : ISlotSchedulerBusiness
    {
        private readonly ISlotRepository slotRepository;
        public SlotSchedulerBusiness(ISlotRepository slotRepository)
        {
            this.slotRepository = slotRepository;
        }
        public async Task<Response<bool>> ScheduleSlot(SlotModel slotModel, string bookedBy)
        {
            
            if (bookedBy == slotModel.CreatedBy)
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotScheduleCannotBookOwnSlot });
            }

            var pendingTimeForSlotMeeting = slotModel.SlotStartZonedDateTime - NodaTimeHelper.GetCurrentUtcZonedDateTime();
            if (pendingTimeForSlotMeeting.Milliseconds < 0)
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotScheduleDateInvalid });
            }

            slotModel.BookedBy = bookedBy;
            slotModel.SlotMeetingLink = CreateSlotMeetingUrl();
            return await this.slotRepository.UpdateSlotBooking(slotModel.Id, slotModel.SlotMeetingLink, slotModel.BookedBy);
        }

        private string CreateSlotMeetingUrl()
        {
            var uniqueId = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var meetingLinkUrl = string.Format("{0}/{1}", SlotConstants.JitsiUrl, uniqueId);

            return meetingLinkUrl;
        }
    }
}
