﻿using Dapper;
using System;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Enitites
{
    [Table("Slot")]
    public class SlotEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        public string BookedBy { get; set; }

        public string TimeZone { get; set; }

        public DateTime SlotDateUtc { get; set; }
        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        //public Money Amount { get; set; }

        public DateTime? ModifiedDateUtc { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public bool IsDeleted { get; set; }

    }
}
