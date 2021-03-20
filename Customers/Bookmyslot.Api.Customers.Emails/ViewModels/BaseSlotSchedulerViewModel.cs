
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Emails.ViewModels
{
    public class BaseSlotSchedulerViewModel
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public string SlotDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string Duration { get; set; }

    
    }
}
