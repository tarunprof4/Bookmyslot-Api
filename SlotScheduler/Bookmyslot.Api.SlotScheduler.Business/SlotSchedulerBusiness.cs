using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class SlotSchedulerBusiness : ISlotSchedulerBusiness
    {
        private readonly ISlotRepository slotRepository;
        private readonly ICustomerRepository customerRepository;
        public SlotSchedulerBusiness(ISlotRepository slotRepository, ICustomerRepository customerRepository)
        {
            this.slotRepository = slotRepository;
            this.customerRepository = customerRepository;
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

            var bookedByCustomerModel = await this.customerRepository.GetCustomerById(bookedBy);
            slotModel.ScheduleSlot(bookedBy);
            return await this.slotRepository.UpdateSlotBooking(slotModel);
        }
    }
}
