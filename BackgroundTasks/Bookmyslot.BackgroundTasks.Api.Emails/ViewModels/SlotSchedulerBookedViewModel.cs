using Bookmyslot.BackgroundTasks.Api.Contracts;

namespace Bookmyslot.BackgroundTasks.Api.Emails.ViewModels
{
    public class SlotSchedulerBookedViewModel : BaseSlotSchedulerViewModel
    {
        public SearchCustomerModel CreatedBy { get;  }

        public SearchCustomerModel BookedBy { get; }
        

        public SlotSchedulerBookedViewModel(SlotModel slotModel, SearchCustomerModel createdBy, SearchCustomerModel BookedBy) :
            base(slotModel)
        {
            this.CreatedBy = createdBy;
            this.BookedBy = BookedBy;
        }

    }
}
