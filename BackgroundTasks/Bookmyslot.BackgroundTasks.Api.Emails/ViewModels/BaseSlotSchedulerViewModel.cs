using Bookmyslot.BackgroundTasks.Api.Contracts;

namespace Bookmyslot.BackgroundTasks.Api.Emails.ViewModels
{
    public abstract class BaseSlotSchedulerViewModel
    {
        public string Title { get; }
        public string Country { get; }
        public string TimeZone { get; }
        public string SlotSlotDate { get; }
        public string SlotStartTime { get; }
        public string SlotEndTime { get; }
        public string SlotDuration { get; }
        public string SlotMeetingLink { get; }

        public BaseSlotSchedulerViewModel(SlotModel slotModel)
        {
            this.Title = slotModel.Title;
            this.Country = slotModel.Country;
            this.TimeZone = slotModel.TimeZone;
            this.SlotSlotDate = slotModel.SlotDate;
            this.SlotStartTime = slotModel.SlotStartTime.ToString();
            this.SlotEndTime = slotModel.SlotEndTime.ToString();
            this.SlotDuration = slotModel.SlotDuration.TotalMinutes.ToString();
            this.SlotMeetingLink = slotModel.SlotMeetingLink;
        }
    }
}
