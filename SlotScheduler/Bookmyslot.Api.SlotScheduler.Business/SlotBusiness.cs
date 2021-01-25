using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.ExtensionMethods;
using Bookmyslot.Api.Common.Contracts.Helpers;
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
        private readonly ICustomerCancelledSlotRepository customerCancelledSlotRepository;

        public SlotBusiness(ISlotRepository slotRepository, ICustomerCancelledSlotRepository customerCancelledSlotRepository)
        {
            this.slotRepository = slotRepository;
            this.customerCancelledSlotRepository = customerCancelledSlotRepository;
        }

        private void SanitizeSlotModel(SlotModel slotModel)
        {
            slotModel.Title = slotModel.Title.Trim();
            slotModel.BookedBy = string.Empty;
        }
        public async Task<Response<Guid>> CreateSlot(SlotModel slotModel)
        {
            slotModel.CreatedBy = UserService.GetUser();

            var currentDate = DateTime.UtcNow;
            var slotDate = DateTimeHelper.ConvertDateStringToDate(slotModel.SlotDate);
            var slotDateWithTimeZone = slotDate.GetDateTimeByTimeZone(slotModel.TimeZone, slotModel.SlotStartTime);
            slotModel.SlotDateUtc = slotDateWithTimeZone.GetDateTimeToUtcByTimeZone(slotModel.TimeZone);

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

        public async Task<Response<bool>> CancelSlot(Guid slotId, string deletedBy)
        {
            if (slotId == Guid.Empty)
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessages.SlotIdInvalid });
            }

            var checkSlotExistsResponse = await CheckIfSlotExists(slotId);
            if (checkSlotExistsResponse.Item1)
            {
                var slotModel = checkSlotExistsResponse.Item2;
                CancelledSlotModel cancelledSlotModel;
                if (deletedBy == slotModel.CreatedBy)
                {
                    var cancelledBy = slotModel.CreatedBy;
                    await this.slotRepository.DeleteSlot(slotModel);
                    cancelledSlotModel = CreateCancelledSlotModel(slotModel, cancelledBy);
                }
                else
                {
                    var cancelledBy = slotModel.BookedBy;
                    slotModel.BookedBy = string.Empty;
                    await this.slotRepository.UpdateSlot(slotModel);
                    cancelledSlotModel = CreateCancelledSlotModel(slotModel, cancelledBy);
                }
                
                await this.customerCancelledSlotRepository.CreateCustomerCancelledSlot(cancelledSlotModel);
                return new Response<bool>() { Result = true };
            }

            return Response<bool>.Empty(new List<string>() { AppBusinessMessages.SlotIdDoesNotExists });
        }

        private CancelledSlotModel CreateCancelledSlotModel(SlotModel slotModel, string cancelledBy)
        {
            return new CancelledSlotModel()
            {
                Id = slotModel.Id,
                Title = slotModel.Title,
                CreatedBy = slotModel.CreatedBy,
                CancelledBy = cancelledBy,
                BookedBy = slotModel.BookedBy,
                TimeZone = slotModel.TimeZone,
                SlotDate = slotModel.SlotDate,
                SlotDateUtc = slotModel.SlotDateUtc,
                SlotStartTime = slotModel.SlotStartTime,
                SlotEndTime = slotModel.SlotEndTime
            };
        }

        public async Task<Response<SlotModel>> GetSlot(Guid slotId)
        {
            if (slotId == Guid.Empty)
            {
                return Response<SlotModel>.ValidationError(new List<string>() { AppBusinessMessages.SlotIdInvalid });
            }

            return await this.slotRepository.GetSlot(slotId);
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
