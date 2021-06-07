using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;
using System;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{
    public class SlotCancelledIntegrationEvent : IntegrationEvent
    {
        public string Title { get; }
        public string Country { get; }
        public string TimeZone { get; }
        public string SlotDate { get; }
        public TimeSpan SlotStartTime { get; }
        public TimeSpan SlotEndTime { get; }
        public TimeSpan SlotDuration { get; }
        public CustomerModel CancelledByCustomerModel { get; }
        public SlotCancelledIntegrationEvent(CancelledSlotModel cancelledSlotModel, CustomerModel cancelledByCustomerModel)
        {
            this.Title = cancelledSlotModel.Title;
            this.Country = cancelledSlotModel.Country;
            this.TimeZone = cancelledSlotModel.SlotStartZonedDateTime.Zone.Id;
            this.SlotDate = NodaTimeHelper.FormatLocalDate(cancelledSlotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationOutPutDatePattern);
            this.SlotStartTime = cancelledSlotModel.SlotStartTime;
            this.SlotEndTime = cancelledSlotModel.SlotEndTime;
            this.SlotDuration = cancelledSlotModel.SlotDuration;
            this.CancelledByCustomerModel = cancelledByCustomerModel;
        }


    }
}
