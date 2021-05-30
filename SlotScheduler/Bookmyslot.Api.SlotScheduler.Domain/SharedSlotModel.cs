using Bookmyslot.Api.Customers.Domain;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Domain
{
    public class SharedSlotModel
    {
        public List<KeyValuePair<CustomerModel, SlotModel>> SharedSlotModels { get; set; }
    }
}
