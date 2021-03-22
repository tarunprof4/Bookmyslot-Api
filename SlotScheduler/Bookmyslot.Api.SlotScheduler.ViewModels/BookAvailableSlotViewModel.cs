using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class BookAvailableSlotViewModel
    {
        public string CreatedFirstName { get; set; }

        public string CreatedByLastName { get; set; }

        public string CreatedByBioHeadLine { get; set; }

        public string ToBeBookedByCountry { get; set; }

        public List<SlotInformationInCustomerTimeZoneViewModel> BookAvailableSlotModels { get; set; }
    }

    
}
