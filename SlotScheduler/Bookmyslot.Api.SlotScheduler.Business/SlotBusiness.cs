using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Business.Validations;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class SlotBusiness : ISlotBusiness
    {
        private readonly ISlotRepository slotRepository;
        public SlotBusiness(ISlotRepository slotRepository)
        {
            this.slotRepository = slotRepository;
        }
        public async Task<Response<Guid>> CreateSlot(SlotModel slotModel)
        {
            var validator = new SlotValidator();
            ValidationResult results = validator.Validate(slotModel);

            if (results.IsValid)
                return await slotRepository.CreateSlot(slotModel);

            else
                return Response<Guid>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
        }

        public async Task<Response<bool>> DeleteSlot(Guid slotId)
        {
            if (slotId == Guid.Empty)
            {
                return  Response<bool>.ValidationError(new List<string>() { AppBusinessMessages.SlotIdInvalid });
            }

            var checkSlotExistsResponse = await CheckIfCustomerExists(slotId);
            if (checkSlotExistsResponse.Item1)
            {
                return await this.slotRepository.DeleteSlot(checkSlotExistsResponse.Item2);
            }

            return Response<bool>.Failed(new List<string>() { AppBusinessMessages.SlotIdDoesNotExists });
        }

        public async Task<Response<IEnumerable<SlotModel>>> GetAllSlotsDateRange(DateTime startDate, DateTime endDate)
        {
            return await this.slotRepository.GetAllSlotsDateRange(DateTime.Now, DateTime.Now);
        }

        public async Task<Response<SlotModel>> GetSlot(Guid slotId)
        {
            if (slotId == Guid.Empty)
            {
                return Response<SlotModel>.ValidationError(new List<string>() { AppBusinessMessages.SlotIdInvalid });
            }

            return await this.slotRepository.GetSlot(slotId);
        }

        public async Task<Response<bool>> UpdateSlot(SlotModel slotModel)
        {
            var validator = new SlotValidator();
            ValidationResult results = validator.Validate(slotModel);

            if (results.IsValid)
            {
                var checkSlotExistsResponse = await CheckIfCustomerExists(slotModel.Id);
                if (checkSlotExistsResponse.Item1)
                {
                    return await this.slotRepository.UpdateSlot(slotModel);
                }

                return Response<bool>.Failed(new List<string>() { AppBusinessMessages.SlotIdDoesNotExists });
            }

            return Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
        }

        private async Task<Tuple<bool, SlotModel>> CheckIfCustomerExists(Guid slotId)
        {
            var slotModelResponse = await this.slotRepository.GetSlot(slotId);
            if (slotModelResponse.HasResult)
                return new Tuple<bool, SlotModel>(true, slotModelResponse.Result);

            return new Tuple<bool, SlotModel>(false, slotModelResponse.Result);
        }
    }
}
