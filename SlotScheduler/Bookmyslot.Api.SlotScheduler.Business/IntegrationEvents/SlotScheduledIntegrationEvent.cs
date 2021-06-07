﻿using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{
    public class SlotScheduledIntegrationEvent : BaseSlotIntegrationEvent
    {
        public CustomerModel CreatedByCustomerModel { get; }
        public CustomerModel BookedByCustomerModel { get; }
        public SlotScheduledIntegrationEvent(SlotModel slotModel, CustomerModel createdByCustomerModel, CustomerModel bookedByCustomerModel)
            :base(slotModel)
        {
            this.CreatedByCustomerModel = createdByCustomerModel;
            this.BookedByCustomerModel = bookedByCustomerModel;
        }
    }
}
