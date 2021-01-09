using Bookmyslot.Api.Customers.Contracts;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class BookedSlotModel
    {
        public CustomerModel CreatedByCustomerModel { get; set; }
        public SlotModel SlotModel { get; set; }

        public string BookedSlotModelInformation { get; set; }
    }
}
