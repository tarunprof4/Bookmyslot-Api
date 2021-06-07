using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class SlotSchedulerBusiness : ISlotSchedulerBusiness
    {
        private readonly ISlotRepository slotRepository;
        private readonly ICustomerBusiness customerBusiness;
        public SlotSchedulerBusiness(ISlotRepository slotRepository, ICustomerBusiness customerBusiness)
        {
            this.slotRepository = slotRepository;
            this.customerBusiness = customerBusiness;
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

            var customerModelsResponse = await this.customerBusiness.GetCustomersByCustomerIds(new List<string>() { slotModel.CreatedBy, bookedBy });

            var createdByCustomerModel = customerModelsResponse.Result.First(a => a.Id == slotModel.CreatedBy);
            var bookedByCustomerModel = customerModelsResponse.Result.First(a => a.Id == bookedBy);

            slotModel.ScheduleSlot(createdByCustomerModel, bookedByCustomerModel);
            return await this.slotRepository.UpdateSlotBooking(slotModel);
        }
    }
}
