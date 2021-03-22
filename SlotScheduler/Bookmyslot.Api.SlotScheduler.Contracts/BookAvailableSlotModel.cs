using Bookmyslot.Api.Customers.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class BookAvailableSlotModel
    {
        public CustomerModel CreatedByCustomerModel { get; set; }

        public List<SlotInforamtionInCustomerTimeZoneModel> AvailableSlotModels { get; set; }

        public CustomerSettingsModel CustomerSettingsModel { get; set; }
    }

 

}
