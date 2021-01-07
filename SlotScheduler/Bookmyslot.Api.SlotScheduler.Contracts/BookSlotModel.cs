
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class BookSlotModel
    {
        public CustomerModel CustomerModel { get; set; }
        public List<BmsKeyValuePair<SlotModel, string>> SlotModelsInforamtion { get; set; }
    }
}
