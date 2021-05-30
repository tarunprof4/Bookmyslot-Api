using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
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
            if (slotModel.IsSlotBookedByHimself(bookedBy))
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotScheduleCannotBookOwnSlot });
            }

            if (!slotModel.IsSlotScheduleDateValid())
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotScheduleDateInvalid });
            }

            slotModel.ScheduleSlot(bookedBy);
            return await this.slotRepository.UpdateSlotBooking(slotModel.Id, slotModel.SlotMeetingLink, slotModel.BookedBy);
        }
    }
}
