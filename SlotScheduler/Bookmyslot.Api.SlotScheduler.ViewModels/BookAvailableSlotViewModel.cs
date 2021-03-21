using Bookmyslot.Api.SlotScheduler.Contracts;
using NodaTime;
using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class BookAvailableSlotViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BioHeadLine { get; set; }

        public List<Tuple<SlotModel, ZonedDateTime, string>> BookAvailableSlotModels { get; set; }
    }
}
