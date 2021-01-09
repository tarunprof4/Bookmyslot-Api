using Bookmyslot.Api.Customers.Contracts;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class SharedSlotModel
    {
        public CustomerModel BookedByCustomerModel { get; set; }
        public SlotModel SlotModel { get; set; }

        public string SharedSlotModelInformation { get; set; }
    }
}
