using Bookmyslot.Api.Customers.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Domain
{
    public class CustomerSlotModel
    {
        public IEnumerable<SlotModel> SlotModels { get; set; }
        public CustomerModel CustomerModel { get; set; }
        
    }
}
