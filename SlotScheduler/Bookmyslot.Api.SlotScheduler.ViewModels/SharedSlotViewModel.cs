using Bookmyslot.Api.SlotScheduler.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class SharedSlotViewModel
    {
        public List<Tuple<CustomerViewModel, SlotModel, string>> SharedSlotModels { get; set; }
        public SharedSlotViewModel()
        {
            SharedSlotModels = new List<Tuple<CustomerViewModel, SlotModel, string>>();
        }
    }
}
