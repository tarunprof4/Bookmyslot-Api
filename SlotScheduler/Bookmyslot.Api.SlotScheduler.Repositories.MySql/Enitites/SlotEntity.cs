using Bookmyslot.Api.Common.Database.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Enitites
{

    [Table(DatabaseConstants.SlotTable)]
    public class SlotEntity
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        public string BookedBy { get; set; }

        public string Country { get; set; }

        public string TimeZone { get; set; }
        public string SlotDate { get; set; }
        public DateTime SlotStartDateTimeUtc { get; set; }

        public DateTime SlotEndDateTimeUtc { get; set; }
        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public DateTime? ModifiedDateUtc { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public bool IsDeleted { get; set; }

    }
}
