using Dapper;
using System;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Enitites
{
    [Table("CancelledSlot")]
    public class CancelledSlotEntity
    {
        
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        [Key]
        public string CancelledBy { get; set; }

        public string TimeZone { get; set; }

        public DateTime SlotDate { get; set; }
        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
