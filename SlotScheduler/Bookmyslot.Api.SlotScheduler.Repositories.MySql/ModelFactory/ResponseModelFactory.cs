using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory
{
    internal class ResponseModelFactory
    {

        internal static Result<SlotModel> CreateSlotModelResponse(SlotEntity slotEntity)
        {
            if (slotEntity == null)
            {
                return Result<SlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.SlotIdDoesNotExists });
            }

            var customerModel = ModelFactory.CreateSlotModel(slotEntity);
            return new Result<SlotModel>() { Value = customerModel };
        }

        internal static Result<CustomerLastSharedSlotModel> CreateCustomerLastSharedSlotModelResponse(CustomerLastSharedSlotEntity customerLastSharedSlotEntity)
        {
            if (customerLastSharedSlotEntity == null)
            {
                return Result<CustomerLastSharedSlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoLastSlotShared });
            }

            var customerModel = ModelFactory.CreateCustomerLastSharedSlotModel(customerLastSharedSlotEntity);
            return new Result<CustomerLastSharedSlotModel>() { Value = customerModel };
        }



        internal static Result<IEnumerable<SlotModel>> CreateSlotModelsResponse(IEnumerable<SlotEntity> slotEntities)
        {
            var slotModels = ModelFactory.CreateSlotModels(slotEntities);
            if (slotModels.Count == 0)
            {
                return Result<IEnumerable<SlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Result<IEnumerable<SlotModel>>() { Value = slotModels };
        }

        internal static Result<IEnumerable<string>> CreateCustomersFromSlotModelsResponse(IEnumerable<SlotEntity> slotEntities)
        {
            var customers = ModelFactory.CreateCustomersFromSlotModels(slotEntities);

            if (customers.Count == 0)
            {
                return Result<IEnumerable<string>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Result<IEnumerable<string>>() { Value = customers };
        }


        internal static Result<IEnumerable<CancelledSlotModel>> CreateCancelledSlotModels(IEnumerable<CancelledSlotEntity> cancelledSlotEntities)
        {
            var slotModels = ModelFactory.CreateCancelledSlotModels(cancelledSlotEntities);
            if (slotModels.Count == 0)
            {
                return Result<IEnumerable<CancelledSlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Result<IEnumerable<CancelledSlotModel>>() { Value = slotModels };
        }

    }

}
