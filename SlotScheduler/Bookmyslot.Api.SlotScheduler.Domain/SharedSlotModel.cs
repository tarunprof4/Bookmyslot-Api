using Bookmyslot.Api.Customers.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Domain
{
    public class SharedSlotModel
    {
        public List<KeyValuePair<CustomerModel, SlotModel>> SharedSlotModels { get; set; }
    }
}
