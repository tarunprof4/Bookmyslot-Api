using NodaTime;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class SlotInforamtionInCustomerTimeZoneModel
    {
        public SlotModel SlotModel { get; set; }
        public ZonedDateTime CustomerSlotZonedDateTime { get; set; }
    }
}
