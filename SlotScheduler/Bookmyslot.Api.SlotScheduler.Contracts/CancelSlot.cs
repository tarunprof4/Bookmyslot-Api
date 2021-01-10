namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class CancelSlot
    {
        public string SlotKey { get; set; }
        public string CancelledBy { get; set; }
    }
}
