using Dapper;
using System;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Enitites
{
    [Table("CancelledSlot")]
    public class CancelledSlotEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        
        public string CancelledBy { get; set; }

        public string BookedBy { get; set; }

        public string TimeZone { get; set; }

        public DateTime SlotDateUtc { get; set; }
        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public DateTime CreatedDateUtc { get; set; }

    }
}
