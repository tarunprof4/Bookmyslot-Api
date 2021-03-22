using Bookmyslot.Api.SlotScheduler.Contracts;
using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class BookedSlotViewModel
    {
        public string ToBeBookedByCustomerCountry { get; set; }

        public List<Tuple<CustomerViewModel, SlotInforamtionInCustomerTimeZoneModel, string>> BookedSlotModels { get; set; }
    }
}
