using Bookmyslot.Api.Customers.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class CustomerSlotModel
    {
        public List<SlotModel> SlotModels { get; set; }
        public CustomerModel CustomerModel { get; set; }

        public string Information { get; set; }
    }
}
