﻿using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSlotRepository
    {
        Task<Response<IEnumerable<string>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel);

        Task<Response<IEnumerable<SlotModel>>> GetCustomerAvailableSlots(PageParameterModel pageParameterModel, string email);

    }
}
