using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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
            return await this.slotRepository.UpdateSlot(slotModel);
        }
    }
}
