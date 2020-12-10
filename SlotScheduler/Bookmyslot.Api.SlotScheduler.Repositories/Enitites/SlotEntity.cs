using Dapper;
using System;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Enitites
{
    [Table("Customer")]
    public class SlotEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan SlotDuration { get; set; }

        //public Money Amount { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

    }
}
