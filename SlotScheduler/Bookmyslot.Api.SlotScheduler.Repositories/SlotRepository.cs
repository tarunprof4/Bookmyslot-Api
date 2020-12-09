using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        public Task<Response<Guid>> CreateSlot(SlotModel slot)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> DeleteSlot(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<SlotModel>>> GetAllSlotsDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SlotModel>> GetSlot(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> UpdateSlot(SlotModel slot)
        {
            throw new NotImplementedException();
        }
    }
}
