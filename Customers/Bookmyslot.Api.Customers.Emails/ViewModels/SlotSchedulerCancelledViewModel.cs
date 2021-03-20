using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;

namespace Bookmyslot.Api.Customers.Emails.ViewModels
{
    public class SlotSchedulerCancelledViewModel : BaseSlotSchedulerViewModel
    {
      
        public string CancelledBy { get; set; }

    }
}
