using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
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
        public async Task<Result<bool>> ScheduleSlot(SlotModel slotModel, string bookedBy)
        {
            if (slotModel.IsSlotBookedByHimself(bookedBy))
            {
                return Result<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotScheduleCannotBookOwnSlot });
            }

            if (!slotModel.IsSlotScheduleDateValid())
            {
                return Result<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotScheduleDateInvalid });
            }

            var customerModelsResponse = await this.customerBusiness.GetCustomersByCustomerIds(new List<string>() { slotModel.CreatedBy, bookedBy });

            var createdByCustomerModel = customerModelsResponse.Value.First(a => a.Id == slotModel.CreatedBy);
            var bookedByCustomerModel = customerModelsResponse.Value.First(a => a.Id == bookedBy);

            slotModel.ScheduleSlot(createdByCustomerModel, bookedByCustomerModel);
            return await this.slotRepository.UpdateSlotBooking(slotModel);
        }
    }
}
