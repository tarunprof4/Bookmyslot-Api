using Bookmyslot.Api.Common.Database.Constants;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Enitites
{

    [Table(DatabaseConstants.CustomerLastBookedSlotTable)]
    public class CustomerLastBookedSlotEntity
    {
        public string CustomerId { get; set; }

        public string Title { get; set; }
        public string Country { get; set; }
        
        public string TimeZone { get; set; }
        public string SlotDate { get; set; }
        public DateTime SlotDateUtc { get; set; }
        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public DateTime ModifiedDateUtc { get; set; }

    }
}
