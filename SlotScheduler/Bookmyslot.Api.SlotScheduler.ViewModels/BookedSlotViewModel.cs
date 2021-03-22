using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class BookedSlotViewModel
    {
        public string ToBeBookedByCustomerCountry { get; set; }

        public List<Tuple<CustomerViewModel, SlotInformationInCustomerTimeZoneViewModel>> BookedSlotModels { get; set; }

        public BookedSlotViewModel()
        {
            this.BookedSlotModels = new List<Tuple<CustomerViewModel, SlotInformationInCustomerTimeZoneViewModel>>();
        }
    }
}
