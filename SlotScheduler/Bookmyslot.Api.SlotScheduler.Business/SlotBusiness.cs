using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
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
        public SlotBusiness(ISlotRepository slotRepository, ICustomerCancelledSlotRepository customerCancelledSlotRepository, ICustomerLastSharedSlotBusiness customerLastBookedSlotBusiness, IValidator<SlotModel> slotModelValidator)
        {
            this.slotRepository = slotRepository;
            this.customerCancelledSlotRepository = customerCancelledSlotRepository;
            this.customerLastBookedSlotBusiness = customerLastBookedSlotBusiness;
            this.slotModelValidator = slotModelValidator;
        }

        private void SanitizeSlotModel(SlotModel slotModel)
        {
            slotModel.Title = slotModel.Title.Trim();
            slotModel.BookedBy = string.Empty;
        }
        public async Task<Response<string>> CreateSlot(SlotModel slotModel, string createdBy)
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
                return Response<string>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
        }



        public async Task<Response<bool>> CancelSlot(string slotId, CustomerSummaryModel cancelledByCustomerSummaryModel)
        {
            if (string.IsNullOrWhiteSpace(slotId))
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotIdInvalid });
            }

            var checkSlotExistsResponse = await CheckIfSlotExists(slotId);
            if (checkSlotExistsResponse.Item1)
            {
                var cancelledBy = cancelledByCustomerSummaryModel.Id;
                var slotModel = checkSlotExistsResponse.Item2;
                var slotStatus = slotModel.CancelSlot(cancelledBy);
                var cancelledSlotModel = CreateCancelledSlotModel(slotModel, cancelledBy);
                cancelledSlotModel.SlotCancelled(cancelledBy);

                var cancelSlotTask = slotStatus == SlotConstants.DeleteSlot ? this.slotRepository.DeleteSlot(slotModel.Id):
                   this.slotRepository.UpdateSlotBooking(slotModel);
                var createCancelledSlotTask = this.customerCancelledSlotRepository.CreateCustomerCancelledSlot(cancelledSlotModel);

                await Task.WhenAll(cancelSlotTask, createCancelledSlotTask);
                return new Response<bool>() { Result = true };
            }

            return Response<bool>.Empty(new List<string>() { AppBusinessMessagesConstants.SlotIdDoesNotExists });
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

        public async Task<Response<SlotModel>> GetSlot(string slotId)
        {
            if (string.IsNullOrWhiteSpace(slotId))
            {
                return Response<SlotModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.SlotIdInvalid });
            }

            return await this.slotRepository.GetSlot(slotId);
        }


        private async Task<Tuple<bool, SlotModel>> CheckIfSlotExists(string slotId)
        {
            var slotModelResponse = await this.slotRepository.GetSlot(slotId);
            if (slotModelResponse.ResultType == ResultType.Success)
                return new Tuple<bool, SlotModel>(true, slotModelResponse.Result);

            return new Tuple<bool, SlotModel>(false, slotModelResponse.Result);
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
