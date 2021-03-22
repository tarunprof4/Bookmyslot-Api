using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class BookAvailableSlotViewModel
    {
        public CustomerViewModel CreatedByCustomerViewModel { get; set; }

        public string ToBeBookedByCustomerCountry { get; set; }

        public List<SlotInformationInCustomerTimeZoneViewModel> BookAvailableSlotModels { get; set; }
    }
}
