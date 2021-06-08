using Bookmyslot.BackgroundTasks.Api.Contracts;

namespace Bookmyslot.BackgroundTasks.Api.Emails.ViewModels
{
    public class SlotSchedulerCancelledViewModel : BaseSlotSchedulerViewModel
    {
        public CustomerModel CancelledBy { get; }
        public CustomerModel NotCancelledBy { get;  }
        public SlotSchedulerCancelledViewModel(SlotModel slotModel, CustomerModel cancelledBy, CustomerModel notcancelledBy) :
          base(slotModel)
        {
            this.CancelledBy = cancelledBy;
            this.NotCancelledBy = notcancelledBy;
        }
    }
}
