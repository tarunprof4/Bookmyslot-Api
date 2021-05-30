﻿using Bookmyslot.Api.Customers.Domain;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Domain
{
    public class BookedSlotModel
    {
        public List<KeyValuePair<CustomerModel, SlotInforamtionInCustomerTimeZoneModel>> BookedSlotModels { get; set; }

        public CustomerSettingsModel CustomerSettingsModel { get; set; }
    }
}
