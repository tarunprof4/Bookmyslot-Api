using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
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
            slotModel.BookedBy = bookedBy;


            if (slotModel.BookedBy == slotModel.CreatedBy)
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotScheduleCannotBookOwnSlot });
            }

            var pendingTimeForSlotMeeting = slotModel.SlotZonedDate - NodaTimeHelper.GetCurrentUtcZonedDateTime();
            if (pendingTimeForSlotMeeting.Milliseconds < 0)
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotScheduleDateInvalid });
            }

            return await this.slotRepository.UpdateSlot(slotModel.Id, slotModel.BookedBy);
        }
    }
}
