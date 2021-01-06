using Bookmyslot.Api.Common;
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

        private void SanitizeSlotModel(SlotModel slotModel)
        {
            slotModel.Title = slotModel.Title.Trim();
            slotModel.SlotDate = slotModel.SlotDate.Date;
        }
        public async Task<Response<Guid>> CreateSlot(SlotModel slotModel)
        {
            var currentDate = DateTime.Now;
            currentDate = currentDate.GetDateTimeByTimeZone(slotModel.TimeZone);
            slotModel.CreatedBy = UserService.GetUser();

            var validator = new SlotValidator(currentDate);
            ValidationResult results = validator.Validate(slotModel);

            if (results.IsValid)
            {
                SanitizeSlotModel(slotModel);
                return await slotRepository.CreateSlot(slotModel);
            }

            else
                return Response<Guid>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
        }

        public async Task<Response<bool>> DeleteSlot(Guid slotId)
        {
            if (slotId == Guid.Empty)
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessages.SlotIdInvalid });
            }

            var checkSlotExistsResponse = await CheckIfSlotExists(slotId);
            if (checkSlotExistsResponse.Item1)
            {
                return await this.slotRepository.DeleteSlot(checkSlotExistsResponse.Item2);
            }

            return Response<bool>.Empty(new List<string>() { AppBusinessMessages.SlotIdDoesNotExists });
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
            var currentDate = DateTime.Now;
            currentDate = currentDate.GetDateTimeByTimeZone(slotModel.TimeZone);

            var validator = new SlotValidator(currentDate);
            ValidationResult results = validator.Validate(slotModel);

            if (results.IsValid)
            {
                SanitizeSlotModel(slotModel);
                var checkSlotExistsResponse = await CheckIfSlotExists(slotModel.Id);
                if (checkSlotExistsResponse.Item1)
                {
                    return await this.slotRepository.UpdateSlot(slotModel);
                }

                return Response<bool>.Empty(new List<string>() { AppBusinessMessages.SlotIdDoesNotExists });
            }

            return Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
        }

        private async Task<Tuple<bool, SlotModel>> CheckIfSlotExists(Guid slotId)
        {
            var slotModelResponse = await this.slotRepository.GetSlot(slotId);
            if (slotModelResponse.ResultType == ResultType.Success)
                return new Tuple<bool, SlotModel>(true, slotModelResponse.Result);

            return new Tuple<bool, SlotModel>(false, slotModelResponse.Result);
        }

        public async Task<Response<IEnumerable<SlotModel>>> GetAllSlots(PageParameterModel pageParameterModel)
        {
            return await this.slotRepository.GetAllSlots(pageParameterModel);
        }
    }
}
