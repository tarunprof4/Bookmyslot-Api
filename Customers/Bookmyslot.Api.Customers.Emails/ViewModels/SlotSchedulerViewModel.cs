using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Emails.ViewModels
{
    public class SlotSchedulerViewModel
    {
        public SlotModel SlotModel { get; set; }

        public CustomerModel CustomerModel { get; set; }
    }
}
