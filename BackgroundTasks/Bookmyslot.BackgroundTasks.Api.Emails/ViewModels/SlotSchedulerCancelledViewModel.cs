using Bookmyslot.BackgroundTasks.Api.Contracts;

namespace Bookmyslot.BackgroundTasks.Api.Emails.ViewModels
{
    public class SlotSchedulerCancelledViewModel : BaseSlotSchedulerViewModel
    {
        public SearchCustomerModel CancelledBy { get; }
        public SearchCustomerModel NotCancelledBy { get;  }
        public SlotSchedulerCancelledViewModel(SlotModel slotModel, SearchCustomerModel cancelledBy, SearchCustomerModel notcancelledBy) :
          base(slotModel)
        {
            this.CancelledBy = cancelledBy;
            this.NotCancelledBy = notcancelledBy;
        }
    }
}
