
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class SlotScheduleModel
    {
        public string  SlotModelKey { get; set; }
        public string BookedBy { get; set; }
    }
}
