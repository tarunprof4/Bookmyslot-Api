using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class BookAvailableSlotViewModel
    {
        public string CreatedByCustomerFirstName { get; set; }

        public string CreatedByCustomerLastName { get; set; }

        public string CreatedByCustomerBioHeadLine { get; set; }

        public string ToBeBookedByCustomerCountry { get; set; }

        public List<SlotInformationInCustomerTimeZoneViewModel> BookAvailableSlotModels { get; set; }
    }

    
}
