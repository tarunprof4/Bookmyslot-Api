using Bookmyslot.Api.Customers.Contracts;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class CancelledSlotInformationModel
    {
       public CancelledSlotModel CancelledSlotModel { get; set; }

       public CustomerModel CancelledByCustomerModel { get; set; }
    }
}
