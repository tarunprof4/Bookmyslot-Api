using NodaTime;

namespace Bookmyslot.Api.SlotScheduler.Domain
{
    public class SlotInforamtionInCustomerTimeZoneModel
    {
        public SlotModel SlotModel { get; set; }
        public ZonedDateTime CustomerSlotZonedDateTime { get; set; }
    }
}
