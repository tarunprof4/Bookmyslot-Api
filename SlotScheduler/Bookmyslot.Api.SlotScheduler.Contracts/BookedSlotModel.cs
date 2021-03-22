using Bookmyslot.Api.Customers.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class BookedSlotModel
    {
        public List<KeyValuePair<CustomerModel, SlotInforamtionInCustomerTimeZoneModel>> BookedSlotModels { get; set; }

        public CustomerSettingsModel CustomerSettingsModel { get; set; }
    }
}
