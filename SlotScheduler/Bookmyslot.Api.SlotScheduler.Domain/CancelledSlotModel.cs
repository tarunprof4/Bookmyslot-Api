using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.DomainEvents;
using Bookmyslot.SharedKernel;
using NodaTime;
using System;

namespace Bookmyslot.Api.SlotScheduler.Domain
{
    public class CancelledSlotModel : BaseEntity<string>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string CreatedBy { get; set; }

        public string CancelledBy { get; set; }

        public string BookedBy { get; set; }

        public string Country { get; set; }
        public ZonedDateTime SlotStartZonedDateTime { get; set; }


        public TimeSpan SlotStartTime { get; set; }


        public TimeSpan SlotEndTime { get; set; }

        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }

        public void SlotCancelled(CustomerModel cancelledByCustomerModel)
        {
            Events.Add(new SlotCancelledDomainEvent(this, cancelledByCustomerModel));
        }

    }
}
