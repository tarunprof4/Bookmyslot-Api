using Bookmyslot.Api.Common.Contracts.Money;
using System;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class SlotModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan SlotDuration { get; set; }

        public Money Amount { get; set; }
    }
}
