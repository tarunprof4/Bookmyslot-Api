using Bookmyslot.Api.Customers.Domain;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Domain
{
    public class BookAvailableSlotModel
    {
        public CustomerModel CreatedByCustomerModel { get; set; }

        public List<SlotInforamtionInCustomerTimeZoneModel> AvailableSlotModels { get; set; }

        public CustomerSettingsModel CustomerSettingsModel { get; set; }
    }



}
