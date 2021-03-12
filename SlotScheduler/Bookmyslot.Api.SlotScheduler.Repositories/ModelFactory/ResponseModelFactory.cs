﻿using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory
{
    internal class ResponseModelFactory
    {

        internal static Response<SlotModel> CreateSlotModelResponse(SlotEntity slotEntity)
        {
            if (slotEntity == null)
            {
                return Response<SlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.SlotIdDoesNotExists });
            }

            var customerModel = ModelFactory.CreateSlotModel(slotEntity);
            return new Response<SlotModel>() { Result = customerModel };
        }

      

        internal static Response<IEnumerable<SlotModel>> CreateSlotModelsResponse(IEnumerable<SlotEntity> slotEntities)
        {
            var slotModels = ModelFactory.CreateSlotModels(slotEntities);
            if (slotModels.Count == 0)
            {
                return Response<IEnumerable<SlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Response<IEnumerable<SlotModel>>() { Result = slotModels };
        }

        internal static Response<IEnumerable<SlotModel>> CreateDistinctCustomersNearestSlotFromTodayResponse(IEnumerable<SlotEntity> slotEntities)
        {
            List<SlotModel> slotModels = new List<SlotModel>();
            foreach (var slotEntity in slotEntities)
            {
                slotModels.Add(new SlotModel() { CreatedBy = slotEntity.CreatedBy });
            }

            if (slotModels.Count == 0)
            {
                return Response<IEnumerable<SlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Response<IEnumerable<SlotModel>>() { Result = slotModels };
        }


        internal static Response<IEnumerable<CancelledSlotModel>> CreateCancelledSlotModels(IEnumerable<CancelledSlotEntity> cancelledSlotEntities)
        {
            var slotModels = ModelFactory.CreateCancelledSlotModels(cancelledSlotEntities);
            if (slotModels.Count == 0)
            {
                return Response<IEnumerable<CancelledSlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound });
            }

            return new Response<IEnumerable<CancelledSlotModel>>() { Result = slotModels };
        }

    }

}
