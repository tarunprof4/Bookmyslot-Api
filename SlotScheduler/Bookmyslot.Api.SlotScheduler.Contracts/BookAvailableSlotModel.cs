using Bookmyslot.Api.Customers.Contracts;
using NodaTime;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class BookAvailableSlotModel
    {
        public CustomerModel CreatedByCustomerModel { get; set; }

        public List<KeyValuePair<SlotModel, ZonedDateTime>> AvailableSlotModels { get; set; }
    }

}
