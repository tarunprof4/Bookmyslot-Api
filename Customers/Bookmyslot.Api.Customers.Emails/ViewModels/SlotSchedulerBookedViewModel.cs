using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;

namespace Bookmyslot.Api.Customers.Emails.ViewModels
{
    public class SlotSchedulerBookedViewModel
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public string SlotDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string Duration { get; set; }

        public string BookedBy { get; set; }

    }
}
