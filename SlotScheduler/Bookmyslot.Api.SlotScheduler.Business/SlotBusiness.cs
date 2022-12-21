using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.ValueObject;
using FluentValidation;
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
        private readonly ICustomerLastSharedSlotBusiness customerLastBookedSlotBusiness;
        private readonly IValidator<SlotModel> slotModelValidator;
        private readonly ICustomerBusiness customerBusiness;
        public SlotBusiness(ISlotRepository slotRepository, ICustomerCancelledSlotRepository customerCancelledSlotRepository,
            ICustomerLastSharedSlotBusiness customerLastBookedSlotBusiness, IValidator<SlotModel> slotModelValidator, ICustomerBusiness customerBusiness)
        {
            this.slotRepository = slotRepository;
            this.customerCancelledSlotRepository = customerCancelledSlotRepository;
            this.customerLastBookedSlotBusiness = customerLastBookedSlotBusiness;
            this.slotModelValidator = slotModelValidator;
            this.customerBusiness = customerBusiness;
        }

        private void SanitizeSlotModel(SlotModel slotModel)
        {
            slotModel.Title = slotModel.Title.Trim();
            slotModel.BookedBy = string.Empty;
        }
        public async Task<Result<string>> CreateSlot(SlotModel slotModel, string createdBy)
        {
            slotModel.CreatedBy = createdBy;

            ValidationResult results = this.slotModelValidator.Validate(slotModel);
            if (results.IsValid)
            {
                SanitizeSlotModel(slotModel);
                var createSlotTask = slotRepository.CreateSlot(slotModel);
                var latestSharedSlotTask = customerLastBookedSlotBusiness.SaveCustomerLatestSharedSlot(CreateCustomerLastBookedSlotModel(slotModel));

                await Task.WhenAll(createSlotTask, latestSharedSlotTask);
                return createSlotTask.Result;
            }

            else
                return Result<string>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
        }



        public async Task<Result<bool>> CancelSlot(string slotId, string cancelledBy)
        {
            if (string.IsNullOrWhiteSpace(slotId))
            {
                return Result<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotIdInvalid });
            }

            var checkSlotExistsResponse = await CheckIfSlotExists(slotId);
            if (checkSlotExistsResponse.Item1)
            {
                var slotModel = checkSlotExistsResponse.Item2;
                var slotStatus = slotModel.CancelSlot(cancelledBy);
                var cancelledSlotModel = CreateCancelledSlotModel(slotModel, cancelledBy);

                var cancelledByCustomerModel = await this.customerBusiness.GetCustomerById(cancelledBy);
                cancelledSlotModel.SlotCancelled(cancelledByCustomerModel.Value);

                var cancelSlotTask = slotStatus == SlotConstants.DeleteSlot ? this.slotRepository.DeleteSlot(slotModel.Id) :
                   this.slotRepository.UpdateSlotBooking(slotModel);
                var createCancelledSlotTask = this.customerCancelledSlotRepository.CreateCustomerCancelledSlot(cancelledSlotModel);

                await Task.WhenAll(cancelSlotTask, createCancelledSlotTask);
                return new Result<bool>() { Value = true };
            }

            return Result<bool>.Empty(new List<string>() { AppBusinessMessagesConstants.SlotIdDoesNotExists });
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
                Country = slotModel.Country,
                SlotStartZonedDateTime = slotModel.SlotStartZonedDateTime,
                SlotStartTime = slotModel.SlotStartTime,
                SlotEndTime = slotModel.SlotEndTime
            };
        }

        public async Task<Result<SlotModel>> GetSlot(string slotId)
        {
            if (string.IsNullOrWhiteSpace(slotId))
            {
                return Result<SlotModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotIdInvalid });
            }

            return await this.slotRepository.GetSlot(slotId);
        }


        private async Task<Tuple<bool, SlotModel>> CheckIfSlotExists(string slotId)
        {
            var slotModelResponse = await this.slotRepository.GetSlot(slotId);
            if (slotModelResponse.ResultType == ResultType.Success)
                return new Tuple<bool, SlotModel>(true, slotModelResponse.Value);

            return new Tuple<bool, SlotModel>(false, slotModelResponse.Value);
        }



        private CustomerLastSharedSlotModel CreateCustomerLastBookedSlotModel(SlotModel slotModel)
        {
            return new CustomerLastSharedSlotModel()
            {
                CreatedBy = slotModel.CreatedBy,
                Title = slotModel.Title,
                Country = slotModel.Country,
                SlotStartZonedDateTime = slotModel.SlotStartZonedDateTime,
                SlotStartTime = slotModel.SlotStartTime,
                SlotEndTime = slotModel.SlotEndTime
            };
        }
    }
}
