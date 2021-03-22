using NodaTime;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class SlotInformationInCustomerTimeZoneViewModel
    {
        public string Title { get; set; }

        public ZonedDateTime SlotStartZonedDateTime { get; set; }

        public ZonedDateTime CustomerSlotStartZonedDateTime { get; set; }

        public string SlotInformation { get; set; }
    }
}
