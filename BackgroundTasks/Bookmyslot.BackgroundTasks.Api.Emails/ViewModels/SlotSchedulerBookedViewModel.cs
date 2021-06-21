using Bookmyslot.BackgroundTasks.Api.Contracts;

namespace Bookmyslot.BackgroundTasks.Api.Emails.ViewModels
{
    public class SlotSchedulerBookedViewModel : BaseSlotSchedulerViewModel
    {
        public CustomerModel CreatedBy { get;  }

        public CustomerModel BookedBy { get; }
        

        public SlotSchedulerBookedViewModel(SlotModel slotModel, CustomerModel createdBy, CustomerModel BookedBy) :
            base(slotModel)
        {
            this.CreatedBy = createdBy;
            this.BookedBy = BookedBy;
        }

    }
}
