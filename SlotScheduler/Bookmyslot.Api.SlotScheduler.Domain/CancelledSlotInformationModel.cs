using Bookmyslot.Api.Customers.Domain;

namespace Bookmyslot.Api.SlotScheduler.Domain
{
    public class CancelledSlotInformationModel
    {
       public CancelledSlotModel CancelledSlotModel { get; set; }

       public CustomerModel CancelledByCustomerModel { get; set; }
    }
}
